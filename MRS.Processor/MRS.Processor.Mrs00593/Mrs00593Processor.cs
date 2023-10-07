using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTranPatiForm;
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

using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
//using MOS.MANAGER.HisTranPati; 
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using MOS.MANAGER.HisTreatmentBedRoom;


namespace MRS.Processor.Mrs00593
{
    public class Mrs00593Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00593RDO> ListRdo = new List<Mrs00593RDO>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        private List<V_HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        private List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_PATIENT_TYPE_ALTER> LastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        public Mrs00593Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00593Filter);
        }

        protected override bool GetData()///
        {
            var filter = ((Mrs00593Filter)reportFilter);
            var result = true;
            try
            {
                HisDepartmentTranViewFilterQuery filtermain = new HisDepartmentTranViewFilterQuery();
                filtermain.DEPARTMENT_IN_TIME_FROM = filter.TIME_FROM;
                filtermain.DEPARTMENT_IN_TIME_TO = filter.TIME_TO;
                filtermain.DEPARTMENT_ID = filter.DEPARTMENT_ID;
                filtermain.DEPARTMENT_IDs = filter.DEPARTMENT_IDs;
                listHisDepartmentTran = new HisDepartmentTranManager(paramGet).GetView(filtermain);
                listHisDepartmentTran = listHisDepartmentTran.Where(o => o.PREVIOUS_ID != null).ToList();
                var ListTreatmentId = listHisDepartmentTran.Select(o => o.TREATMENT_ID).Distinct().ToList();

                //Đối tượng điều trị

                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery()
                        {
                            IDs = listIDs,
                            ORDER_DIRECTION = "ASC",
                            ORDER_FIELD = "ID"
                        };
                        var LisTreatmentLib = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                        ListTreatment.AddRange(LisTreatmentLib);
                        HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            ORDER_DIRECTION = "ASC",
                            ORDER_FIELD = "ID"
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                        ListPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                    LastPatientTypeAlter = ListPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q=>q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    listHisDepartmentTran = listHisDepartmentTran.Where(o => LastPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && filter.TREATMENT_TYPE_IDs.Contains(p.TREATMENT_TYPE_ID))).ToList();
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
            var result = true;
            try
            {
                ListRdo.Clear();

                foreach (var item in listHisDepartmentTran)
                {
                    List<HIS_PATIENT_TYPE_ALTER> patientTypeAlterSub = ListPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == item.PREVIOUS_ID && o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                    if (patientTypeAlterSub.Count == 0) continue;
                    var treatment = ListTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                    Mrs00593RDO rdo = new Mrs00593RDO(item, treatment);

                    var listPatientTypeAlterSub = LastPatientTypeAlter.Where(o => o.TREATMENT_ID == item.TREATMENT_ID && o.HEIN_CARD_NUMBER != null).ToList();
                    if (IsNotNullOrEmpty(listPatientTypeAlterSub))
                    {
                        rdo.IS_BHYT = "x";
                    }
                    
                    ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
     
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00593Filter)reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00593Filter)reportFilter).TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ((Mrs00593Filter)reportFilter).DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME);

            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o=>o.DEPARTMENT_IN_TIME).ToList());
        }

    }


}
