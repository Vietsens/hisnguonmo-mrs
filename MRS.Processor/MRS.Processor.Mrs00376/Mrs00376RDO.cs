using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00376
{
    public class Mrs00376RDO
    {
        public string RESULT_DATE_STR { get;  set;  }
        public long RESULT_TIME { get;  set;  }
        public long RESULT_DATE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public long PATIENT_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }

        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string ICD_CODE { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string HIV_RESULT { get;  set;  }
        public string HBS_RESULT { get;  set;  }

        public Mrs00376RDO() { }

        public Mrs00376RDO(V_HIS_SERVICE_REQ serviceReq)
        {
            if (serviceReq != null)
            {
                this.RESULT_TIME = serviceReq.FINISH_TIME.Value; 
                this.RESULT_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.RESULT_TIME); 
                this.RESULT_DATE = Convert.ToInt64(this.RESULT_TIME.ToString().Substring(0, 8)); 
                this.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE; 
                this.PATIENT_ID = serviceReq.TDL_PATIENT_ID; 
                this.VIR_PATIENT_NAME = serviceReq.TDL_PATIENT_NAME; 
                this.VIR_ADDRESS = serviceReq.TDL_PATIENT_ADDRESS; 
                this.TREATMENT_ID = serviceReq.TREATMENT_ID; 
                this.TREATMENT_CODE = serviceReq.TREATMENT_CODE; 
                this.ICD_CODE = serviceReq.ICD_CODE; 
                this.ICD_NAME = serviceReq.ICD_NAME; 
                if (serviceReq.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.FEMALE_YEAR = serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                }
                else
                {
                    this.MALE_YEAR = serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                }
            }
        }
    }
}
