using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisExpMest; 

namespace MRS.Processor.Mrs00404
{
    class Mrs00404Processor : AbstractProcessor
    {
        Mrs00404Filter castFilter = null; 
        List<Mrs00404RDO> listRdo = new List<Mrs00404RDO>(); 

        public Mrs00404Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterGroups = new List<V_HIS_PATIENT_TYPE_ALTER>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00404Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00404Filter)this.reportFilter; 

                var skip = 0; 

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                treatmentViewFilter.IN_TIME_FROM = castFilter.TIME_FROM; 
                treatmentViewFilter.IN_TIME_TO = castFilter.TIME_TO; 

                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentViewFilter); 

                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList(); 
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listTreatmentId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterFilter.TREATMENT_IDs = listTreatmentId; 
                    patientTypeAlterFilter.ORDER_DIRECTION = "DESC"; 
                    patientTypeAlterFilter.ORDER_FIELD = "LOG_TIME"; 

                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter); 

                    listPatientTypeAlters.AddRange(listPatientTypeAlter); 
                }
                foreach(var listPatientTypeAlter_ in listPatientTypeAlters.GroupBy(x => x.TREATMENT_ID))
                {
                    var patientTypeAlter = listPatientTypeAlter_.First(); 
                    listPatientTypeAlterGroups.Add(patientTypeAlter); 
                }
                
                var listTreatmentIds_ = listPatientTypeAlterGroups.Where(w=>w.TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(s => s.TREATMENT_ID).ToList(); 
                listTreatments = listTreatments.Where(w => listTreatmentIds_.Contains(w.ID)).ToList(); 
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
                if (IsNotNullOrEmpty(listTreatments))
                {
                    foreach (var treatment in listTreatments)
                    {
                        var patientTypeAlter = listPatientTypeAlters.FirstOrDefault(w => w.TREATMENT_ID == treatment.ID); 
                        if (patientTypeAlter==null)
                        {
                            continue; 
                        }

                        if (treatment.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            continue;
                        }
                        Mrs00404RDO rdo = new Mrs00404RDO(); 
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE; 
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                        rdo.AGE = treatment.TDL_PATIENT_DOB; 
                        rdo.GENDER = treatment.TDL_PATIENT_GENDER_NAME; 
                        rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS; 
                        if (patientTypeAlter.PATIENT_TYPE_ID==MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                                rdo.HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER; 
                        }
                        rdo.IN_TIME =Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME); 
                        rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME??0); 
                        rdo.DEPARTMENT_EXCUTE = treatment.END_DEPARTMENT_NAME;
                        if (treatment.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            rdo.NOT_OUT = "X";
                        }
                        if (treatment.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            rdo.NOT_LOCK_FEE = "X";
                        }
                       
                        listRdo.Add(rdo); 
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
