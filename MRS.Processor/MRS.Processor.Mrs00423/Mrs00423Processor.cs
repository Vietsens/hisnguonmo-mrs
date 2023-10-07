using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00423
{
    class Mrs00423Processor : AbstractProcessor
    {
        Mrs00423Filter filter = null; 

        List<Mrs00423RDO> ListRdo = new List<Mrs00423RDO>(); 
        List<Mrs00423RDO> Parent = new List<Mrs00423RDO>(); 

        List<V_HIS_TREATMENT_4> ListTreatments = new List<V_HIS_TREATMENT_4>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<HIS_ICD> listIcd = new List<HIS_ICD>(); 
        public Mrs00423Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00423Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.filter = (Mrs00423Filter)this.reportFilter; 
                //phong
                HisRoomFilterQuery fRoom = new HisRoomFilterQuery(); 
                fRoom.DEPARTMENT_ID = filter.DEPARTMENT_ID; 
                var listRoom = new HisRoomManager().Get(fRoom); 

                //HSDT
                HisTreatmentView4FilterQuery treatmentViewFilter = new HisTreatmentView4FilterQuery(); 
                treatmentViewFilter.OUT_TIME_FROM = filter.TIME_FROM; 
                treatmentViewFilter.OUT_TIME_TO = filter.TIME_TO; 
                if (IsNotNullOrEmpty(listRoom)) treatmentViewFilter.END_ROOM_IDs = listRoom.Select(o => o.ID).ToList(); 
                ListTreatments = new HisTreatmentManager(param).GetView4(treatmentViewFilter); 

                //loc theo bac si
                if (IsNotNullOrEmpty(filter.DOCTOR_LOGINNAMEs)) ListTreatments.Where(o => filter.DOCTOR_LOGINNAMEs.Contains(o.DOCTOR_LOGINNAME)).ToList(); 
                if (filter.DOCTOR_LOGINNAME!=null) ListTreatments.Where(o => filter.DOCTOR_LOGINNAME==o.DOCTOR_LOGINNAME).ToList(); 

                //icd
                HisIcdFilterQuery fIcd = new HisIcdFilterQuery(); 
                listIcd = new HisIcdManager().Get(fIcd); 

                //chuyen doi tuong
                if (IsNotNullOrEmpty(ListTreatments))
                {
                    var skip = 0; 
                    while (ListTreatments.Count - skip > 0)
                    {
                        var listIds = ListTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));  
                    }
                }

                result = true; 
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
                foreach (var treatment in ListTreatments)
                {
                    Mrs00423RDO rdo = new Mrs00423RDO(); 
                    rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                    rdo.DOB_YEAR_MALE = treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE ? treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) : ""; 
                    rdo.DOB_YEAR_FEMALE = treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                    rdo.TREAT_DAY = DateDiff.diffDate(treatment.IN_TIME, treatment.OUT_TIME); 
                    var department = HisDepartmentCFG.DEPARTMENTs.Where(o => o.ID == treatment.END_DEPARTMENT_ID).ToList(); 
                    rdo.END_DEPARTMENT_NAME = department.Count>0?department.First().DEPARTMENT_NAME:""; 
                    var patientTypeAlterSub = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatment.ID).ToList(); 
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        var BhytCard = patientTypeAlterSub.Where(s => s.HEIN_CARD_NUMBER != null).ToList(); 
                       if (IsNotNullOrEmpty(BhytCard)) rdo.HEIN_CARD_NUMBER = BhytCard.First().HEIN_CARD_NUMBER; 
                    }
                    rdo.IN_TIME_STR=Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME); 
                    rdo.OUT_TIME_STR=Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME??0);
                    var listIcdSub = listIcd.Where(o => o.ICD_CODE == treatment.ICD_CODE).ToList(); 
                    rdo.ICD_NAME=	listIcdSub.Count>0? listIcdSub.First().ICD_NAME:""; 
                    rdo.DOCTOR_USERNAME = treatment.DOCTOR_USERNAME; 
                    rdo.DOCTOR_LOGINNAME = treatment.DOCTOR_LOGINNAME; 

                    ListRdo.Add(rdo); 
                }
                Parent = ListRdo.GroupBy(o => o.DOCTOR_LOGINNAME).Select(p => p.First()).ToList(); 
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
                if (filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM)); 
                }
                if (filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO)); 
                }
                objectTag.AddObjectData(store, "Report", ListRdo); 
                objectTag.AddObjectData(store, "Parent", Parent); 
                objectTag.AddRelationship(store, "Parent", "Report", "DOCTOR_LOGINNAME", "DOCTOR_LOGINNAME"); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
       
    }
}
