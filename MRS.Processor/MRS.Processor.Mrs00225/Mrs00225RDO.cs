using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00225
{
    public class Mrs00225RDO
    {

        public Mrs00225RDO(V_HIS_TREATMENT treatment, V_HIS_SERE_SERV sereServ, HIS_SERVICE service, List<V_HIS_EKIP_USER> ekipUsers)
        {

            var AGE_MEN = "";
            var AGE_WOMEN = "";
            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            {
                AGE_MEN = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
            }
            else if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                AGE_WOMEN = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
            }
            var address = treatment.TDL_PATIENT_ADDRESS;
            this.PATIENT_ID = treatment.TDL_PATIENT_CODE;
            this.TREATMENT_CODE = treatment.TREATMENT_CODE;
            this.PATIENT_MANE = treatment.TDL_PATIENT_NAME;
            this.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
            this.ICD_TREATMENT = treatment.ICD_CODE + "" + treatment.ICD_SUB_CODE;
            this.MALE = AGE_MEN;
            this.FEMALE = AGE_WOMEN;
            this.ADDRESS = address;

            this.HEIN = sereServ.HEIN_CARD_NUMBER;
            this.EXECUTE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServ.TDL_INTRUCTION_TIME);
            this.SERVICE_REQ_CODE = sereServ.TDL_SERVICE_REQ_CODE;
            this.REQUEST_USERNAME = sereServ.TDL_REQUEST_USERNAME;
            this.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

            var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID);
            if(ptttGroup != null)
            {
                this.PTTT_GROUP_CODE = ptttGroup.PTTT_GROUP_CODE;
                this.PTTT_GROUP_NAME = ptttGroup.PTTT_GROUP_NAME;
            }

            var doctorPttt = "";
            var doctorGm = "";
            if (ekipUsers!= null && ekipUsers.Count>0)
                foreach (var ekipUser in ekipUsers)
                {
                    {
                        if ((ekipUser.EXECUTE_ROLE_ID) == HisExecuteRoleCFG.HisExecuteRoleId__TT)
                        {
                            doctorPttt = ekipUser.USERNAME;
                        }

                        if ((ekipUser.EXECUTE_ROLE_ID) == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist)
                        {
                            doctorGm = ekipUser.USERNAME;
                        }
                    }
                }

            this.DOCTOR_NAME_PTTT = doctorPttt;
            this.DOCTOR_NAME_GM = doctorGm;

            if (service != null)
            {
                if (service.SERVICE_NAME.ToLower().Contains("cấy chỉ"))
                {
                    this.TYPE_CODE = "CC";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("điện châm"))
                {
                    this.TYPE_CODE = "DC";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("thủy châm") || service.SERVICE_NAME.ToLower().Contains("thuỷ châm"))
                {
                    this.TYPE_CODE = "TC";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("xoa bóp"))
                {
                    this.TYPE_CODE = "XB";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("giác hơi"))
                {
                    this.TYPE_CODE = "GH";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("cứu"))
                {
                    this.TYPE_CODE = "HELP";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("ngâm"))
                {
                    this.TYPE_CODE = "SOAK";
                }
                else if (service.SERVICE_NAME.ToLower().Contains("hào châm"))
                {
                    this.TYPE_CODE = "HC";
                }
            }
            this.TDL_SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
            this.TDL_SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
        }

        public long STT { get;  set;  }
        public string PATIENT_ID { get;  set;  }
        public string PATIENT_MANE { get;  set;  }
        public string MALE { get;  set;  }
        public string FEMALE { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string HEIN { get;  set;  }
        public string BEFORE_PTTT_ICD_NAME { get;  set;  }
        public string AFTER_PTTT_ICD_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public string PTTT_METHOD_NAME { get;  set;  }
        public string EMOTIONLESS_METHOD_NAME { get;  set;  }
        public string EXECUTE_TIME { get;  set;  }
        public string PTTT_GROUP_NAME { get;  set;  }
        public string PTTT_GROUP_CODE { get; set; }
        public string DOCTOR_NAME_PTTT { get;  set;  }
        public string DOCTOR_NAME_GM { get;  set;  }
        public string NOTE { get;  set;  }

        public string TREATMENT_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string ICD_TREATMENT { get; set; }

        public string TYPE_CODE { get; set; }

        public long SERVICE_REQ_ID { get; set; }

        public string SERVICE_REQ_CODE { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
    }
}
