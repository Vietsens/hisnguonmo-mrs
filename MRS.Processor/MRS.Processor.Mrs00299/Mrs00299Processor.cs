using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisMediStock;
using SDA.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00299
{
    public class Mrs00299Processor : AbstractProcessor
    {
        Mrs00299Filter castFilter = null;
        private List<Mrs00299RDO> ListRdo = new List<Mrs00299RDO>();
        private List<HIS_EXP_MEST> ListPrescription = new List<HIS_EXP_MEST>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        private List<HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        CommonParam paramGet = new CommonParam();

        public Mrs00299Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00299Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            try
            {
                this.castFilter = (Mrs00299Filter)this.reportFilter;
                //get dữ liệu:
                var listMediStockIds = MANAGER.Config.HisMediStockCFG.HisMediStocks.Where(o => !HisDepartmentCFG.DEPARTMENTs.Exists(p => p.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && p.ID == o.DEPARTMENT_ID)).Select(p => p.ID).ToList();

                if (IsNotNullOrEmpty(this.castFilter.MEDI_STOCK_IDs))
                {
                    listMediStockIds = this.castFilter.MEDI_STOCK_IDs;
                }
                HisExpMestFilterQuery expMestMedicinefilter = new HisExpMestFilterQuery()
                {
                    FINISH_TIME_FROM = this.castFilter.TIME_FROM,
                    FINISH_TIME_TO = this.castFilter.TIME_TO,
                    EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    MEDI_STOCK_IDs = listMediStockIds,
                    EXP_MEST_TYPE_IDs = new List<long>()
                        {
                            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                        }
                };
                ListPrescription = new HisExpMestManager(paramGet).Get(expMestMedicinefilter);

                if (IsNotNullOrEmpty(ListPrescription))
                {
                    List<long> ListTreatmentId = ListPrescription.Select(q => q.TDL_TREATMENT_ID.Value).Distinct().ToList();
                    if (IsNotNullOrEmpty(ListTreatmentId))
                    {
                        var skip = 0;
                        while (ListTreatmentId.Count - skip > 0)
                        {
                            var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery ListTreatmentfilter = new HisTreatmentFilterQuery()
                            {
                                IDs = listIDs
                            };
                            var listTreatmentSub = new HisTreatmentManager(paramGet).Get(ListTreatmentfilter);
                            ListTreatment.AddRange(listTreatmentSub);

                            //Lấy đối tượng để xem các đối tượng đó 
                            HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                            {
                                TREATMENT_IDs = listIDs
                            };
                            var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                            LisPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                        }

                        //Loại bỏ các trường hợp là nội trú và điều trị ngoại trú
                        var LisPatientTypeAlterne = LisPatientTypeAlter.Where(o =>
                            o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ||
                            o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                        if (IsNotNullOrEmpty(LisPatientTypeAlterne))
                        {
                            List<long> ListTreatmentId2 = LisPatientTypeAlterne.Select(o => o.TREATMENT_ID).ToList();
                            ListTreatment = ListTreatment.Where(o => !ListTreatmentId2.Contains(o.ID)).ToList();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {

                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    foreach (var treat in ListTreatment)
                    {
                        Mrs00299RDO rdo = new Mrs00299RDO(treat);
                        rdo.MEDICINE_PRICE = ListPrescription.Where(o => o.TDL_TREATMENT_ID == treat.ID).Sum(s => s.TDL_TOTAL_PRICE ?? 0);
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
        }

    }
}
