using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00325
{
    class Mrs00325Processor : AbstractProcessor
    {
        Mrs00325Filter castFilter = null;

        List<Mrs00325RDO> listRdo = new List<Mrs00325RDO>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();  // Ds thông tin HSBA 
        List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();  // Ds Bệnh nhân nội trú
        List<V_HIS_PATIENT> ListPatient = new List<V_HIS_PATIENT>();  // Ds Bệnh nhân
        public Mrs00325Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00325Filter);
        }
        //get dữ liệu từ data base
        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00325Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                //Lọc theo thời gian bảng:V_HIS_TREATMENT
                HisTreatmentFilterQuery TreatmentFilter = new HisTreatmentFilterQuery(); //khai báo filter dữ liệu theo thời gian
                TreatmentFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                TreatmentFilter.IN_TIME_TO = castFilter.TIME_TO;
                listTreatment = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                var listPatientID = listTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                var listTreatmentID = listTreatment.Select(o => o.ID).ToList();
                // Get bảng V_HIS_PATIENT
                var skip = 0;
                while (listPatientID.Count - skip > 0)
                {
                    var listIds = listPatientID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var FilterHisPatientViewFilterQuery = new HisPatientViewFilterQuery
                    {
                        IDs = listIds
                    };
                    var listPatientSub = new HisPatientManager(paramGet).GetView(FilterHisPatientViewFilterQuery);
                    ListPatient.AddRange(listPatientSub);
                }


                //Get bảng V_HIS_PATIENT_TYPE_ALTER 
                skip = 0;
                while (listTreatmentID.Count - skip > 0)
                {
                    var listIds = listTreatmentID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var PatientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                    };
                    var listPatientTypeSub = new HisPatientTypeAlterManager(paramGet).Get(PatientTypeAlterFilter);
                    ListPatientTypeAlter.AddRange(listPatientTypeSub);
                }



                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00325");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        //Xử lý dữ liệu để tạo listRdo
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                listRdo.Clear();
                var treatmentGroupbyProvine = listTreatment.GroupBy(o => (ListPatient.FirstOrDefault(p => p.ID == o.PATIENT_ID) ?? new V_HIS_PATIENT()).PROVINCE_CODE).ToList();
                foreach (var group in treatmentGroupbyProvine)
                {
                    List<HIS_TREATMENT> listSub = group.ToList<HIS_TREATMENT>();
                    Mrs00325RDO rdo = new Mrs00325RDO();
                    rdo.PROVINCE_NAME = (ListPatient.FirstOrDefault(p => p.PROVINCE_CODE == group.Key) ?? new V_HIS_PATIENT()).PROVINCE_NAME;

                    rdo.NUMBER_PARTIENT = listSub.Count;
                    rdo.NUMBER_BHYT = listSub.Where(o => ListPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.ID )).ToList().Count;

                    listRdo.Add(rdo);
                }
                result = true;

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdo.Clear();

            }
            return result;
        }

        // xuất ra báo cáo
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
