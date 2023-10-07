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

namespace MRS.Processor.Mrs00374
{
    class Mrs00374Processor : AbstractProcessor
    {
        Mrs00374Filter castFilter = null; 
        List<Mrs00374RDO> listRdo = new List<Mrs00374RDO>(); 

        public Mrs00374Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00374Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00374Filter)this.reportFilter; 

                var skip = 0; 

                //v_his_patient_type_alter
                HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                patientTypeAlterFilter.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU; 
                listPatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter); 

                if (IsNotNullOrEmpty(listPatientTypeAlters))
                {
                    while (listPatientTypeAlters.Count - skip > 0)
                    {
                        // v_his_treatment
                        var listTreatmentIDs = listPatientTypeAlters.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                        treatmentViewFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                        treatmentViewFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                        treatmentViewFilter.IDs = listTreatmentIDs.Select(s => s.TREATMENT_ID).ToList(); 

                        var listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentViewFilter); 

                        listTreatments.AddRange(listTreatment); 

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

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                if (IsNotNullOrEmpty(listTreatments))
                {
                    foreach (var treatment in listTreatments)
                    {
                        Mrs00374RDO rdo = new Mrs00374RDO(); 
                        rdo.ICD = treatment.ICD_CODE.Substring(0,3); 
                        if(treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.TOTAL_MALE = 1; 
                        }
                        if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.TOTAL_FEMALE = 1; 
                        }
                        rdo.TOTAL = 1;
                        rdo.TREATMENT_TIME = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                     
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
