using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using HIS.Treatment.DateTime; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisTreatment; 

namespace MRS.Processor.Mrs00458
{
    class Mrs00458Processor : AbstractProcessor
    {
        Mrs00458Filter castFilter = null; 
        List<Mrs00458RDO> listRdo = new List<Mrs00458RDO>(); 
        List<Mrs00458RDO> listRdoGroupRoom = new List<Mrs00458RDO>(); 
        List<Mrs00458RDO> listRdoGroupFinishTime = new List<Mrs00458RDO>(); 
        List<V_HIS_SERVICE_REQ> listExamServiceReqs = new List<V_HIS_SERVICE_REQ>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, V_HIS_TREATMENT> dicTreatment= new Dictionary<long, V_HIS_TREATMENT>(); 
        string thisReportTypeCode = ""; 
        public Mrs00458Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00458Filter); 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "Report", listRdo); 
                objectTag.AddObjectData(store, "listRdoGroupRoom", listRdoGroupRoom); 
                objectTag.AddObjectData(store, "listRdoGroupFinishTime", listRdoGroupFinishTime); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupRoom", "listRdoGroupFinishTime", "DEPARTMENT_CODE", "DEPARTMENT_CODE"); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupRoom", "listRdoGroupFinishTime", new string[] { "DEPARTMENT_CODE", "ROOM_CODE" }, new string[] { "DEPARTMENT_CODE", "ROOM_CODE" }); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupFinishTime", "Report", new string[] { "DEPARTMENT_CODE", "ROOM_CODE", "FINISH_TIME" }, new string[] { "DEPARTMENT_CODE", "ROOM_CODE", "FINISH_TIME" }); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00458Filter)this.reportFilter; 

                HisServiceReqViewFilterQuery examServiceReqFilter = new HisServiceReqViewFilterQuery(); 
                examServiceReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM; 
                examServiceReqFilter.FINISH_TIME_TO = castFilter.TIME_TO; 
                examServiceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH; 
                examServiceReqFilter.EXECUTE_ROOM_ID = castFilter.EXAM_ROOM_ID; 
                listExamServiceReqs = new HisServiceReqManager(param).GetView(examServiceReqFilter); 

                var listTreatmentIds = listExamServiceReqs.Select(o => o.TREATMENT_ID).Distinct().ToList();
                dicTreatment = new HisTreatmentManager().GetViewByIds(listTreatmentIds).ToDictionary(o => o.ID); 
                dicPatyAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatmentIds).ToDictionary(p => p.ID); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                V_HIS_TREATMENT treatment = null; 
                foreach (var examServiceReqs in listExamServiceReqs)
                {
                    if (!dicTreatment.ContainsKey(examServiceReqs.TREATMENT_ID)) continue; 

                    treatment = dicTreatment[examServiceReqs.TREATMENT_ID]; 
                    Mrs00458RDO rdo = new Mrs00458RDO(treatment); 
                    rdo.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(examServiceReqs.FINISH_TIME ?? 0); 
                    rdo.DEPARTMENT_CODE = examServiceReqs.EXECUTE_DEPARTMENT_ID; 
                    rdo.ROOM_CODE = examServiceReqs.EXECUTE_ROOM_CODE; 
                    rdo.ROOM_NAME = examServiceReqs.EXECUTE_ROOM_NAME; 
                    rdo.TREATMENT_CODE = examServiceReqs.TREATMENT_CODE; 
                    rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE; 
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                    rdo.BIRTH_DAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(examServiceReqs.TDL_PATIENT_DOB); 
                    rdo.DOB_STR = examServiceReqs.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                    rdo.GENDER_STR = examServiceReqs.TDL_PATIENT_GENDER_NAME; 
                    rdo.HEIN_CARD_NUMBER = dicPatyAlter.ContainsKey(examServiceReqs.TREATMENT_ID) ?
                    dicPatyAlter[examServiceReqs.TREATMENT_ID].HEIN_CARD_NUMBER : ""; 
                    rdo.ADDRESS = examServiceReqs.TDL_PATIENT_ADDRESS; 
                    
                    listRdo.Add(rdo); 
                }

                listRdoGroupFinishTime = listRdo.GroupBy(s => new { s.ROOM_CODE, s.DEPARTMENT_CODE, s.FINISH_TIME }).Select(s => new Mrs00458RDO
                {
                    DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                    //DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    ROOM_CODE = s.First().ROOM_CODE,
                    ROOM_NAME = s.First().ROOM_NAME,
                    FINISH_TIME = s.First().FINISH_TIME
                }).ToList(); 

                listRdoGroupRoom = listRdo.GroupBy(s => new { s.ROOM_CODE, s.DEPARTMENT_CODE}).Select(s => new Mrs00458RDO
                {
                    DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                    //DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    ROOM_CODE = s.First().ROOM_CODE,
                    ROOM_NAME = s.First().ROOM_NAME,
                    FINISH_TIME = s.First().FINISH_TIME
                }).ToList(); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }


    }
}
