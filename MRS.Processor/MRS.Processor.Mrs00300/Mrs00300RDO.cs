using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00300
{
    public class Mrs00300RDO : V_HIS_TREATMENT
    {
        public int? MALE_AGE { get;  set;  }
        public int? FEMALE_AGE { get;  set;  }
        public int? MALE_YEAR { get;  set;  }
        public int? FEMALE_YEAR { get;  set;  }
        public string IS_OFFICER { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_CITY { get;  set;  }
        public string IS_COUNTRY_SIDE { get;  set;  }
        public string UNDER_12_MONTH { get;  set;  }
        public string UNDER_15_YEAR { get;  set;  }
        public string JOB { get;  set;  }
        public string TRAN_PATI_NAME { get;  set;  }
        public string TRAN_PATI_CODE { get;  set;  }

        public string IN_DATE_STR { get;  set;  }
        public string TRAN_DATE_STR { get;  set;  }
        public string OUT_DATE_STR { get;  set;  }

        public string DIAGNOSE_DOWNLINE { get;  set;  }
        public string DIAGNOSE_EXAM { get;  set;  }
        public string DIAGNOSE_TREAT { get;  set;  }
        public string DIAGNOSE_PATHOLOGY { get;  set;  }

        public string TRAN_ICD_CODE { get;  set;  }
        public string TRAN_ICD_NAME { get;  set;  }
        public string TRAN_ICD_SUB_CODE { get;  set;  }
        public string TRAN_ICD_TEXT { get;  set;  }

        public string EXAM_ICD_CODE { get;  set;  }
        public string EXAM_ICD_NAME { get;  set;  }
        public string EXAM_ICD_SUB_CODE { get;  set;  }
        public string EXAM_ICD_TEXT { get;  set;  }

        public string PATHOLOGY_ICD_CODE { get;  set;  }
        public string PATHOLOGY_ICD_NAME { get;  set;  }
        public string PATHOLOGY_ICD_SUB_CODE { get;  set;  }
        public string PATHOLOGY_ICD_TEXT { get;  set;  }

        public string IS_CURED { get;  set;  }
        public string IS_REDUCE { get;  set;  }
        public string IS_HEAVIER { get;  set;  }
        public string IS_CONSTANT { get;  set;  }

        public string SURG_PTTT_GROUP_NAMEs { get;  set;  }	
        public string MISU_PTTT_GROUP_NAMEs { get;  set;  }		
        public string USERNAMEs { get;  set;  }	
        public string PTTT_METHOD_NAMEs { get;  set;  }	
        public string EMOTIONLESS_METHOD_NAMEs { get;  set;  }



        public Mrs00300RDO(V_HIS_TREATMENT data)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00300RDO>(this, data); 
                this.ProcessAgeAndYear(); 
                this.ProcessDateInOut();
                this.ProcessResult();
                this.ProcessIcd();
            }
        }

        void ProcessAgeAndYear()
        {
            int? tuoi = RDOCommon.CalculateAge(this.TDL_PATIENT_DOB); 
            if (tuoi >= 0)
            {
                if (this.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    this.MALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                    this.MALE_YEAR = int.Parse(this.TDL_PATIENT_DOB.ToString().Substring(0, 4)); 
                }
                else
                {
                    this.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                    this.FEMALE_YEAR = int.Parse(this.TDL_PATIENT_DOB.ToString().Substring(0, 4)); 
                }
            }
            if (tuoi == 0)
            {
                this.UNDER_12_MONTH = "X"; 
            }
            else if (tuoi >= 1 && tuoi <= 15)
            {
                this.UNDER_15_YEAR = "X"; 
            }
        }

        void ProcessDateInOut()
        {
            this.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.CLINICAL_IN_TIME ?? 0); 
            if (this.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
            {
                this.TRAN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.OUT_TIME ?? 0); 
            }
            else
            {
                this.OUT_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.OUT_TIME ?? 0); 
            }
        }

        void ProcessResult()
        {
            if (this.TREATMENT_RESULT_ID.HasValue)
            {
                if (this.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                {
                    this.IS_CURED = "X"; 
                }
                else if (this.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                {
                    this.IS_REDUCE = "X"; 
                }
                else if (this.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                {
                    this.IS_HEAVIER = "X"; 
                }
                else if (this.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                {
                    this.IS_CONSTANT = "X"; 
                }
            }
        }

        void ProcessIcd()
        {
            this.DIAGNOSE_TREAT = this.ICD_NAME;
        }


        public string IS_SURG_PTTT_GROUP_1 { get; set; }

        public string IS_SURG_PTTT_GROUP_4 { get; set; }

        public string IS_SURG_PTTT_GROUP_3 { get; set; }

        public string IS_SURG_PTTT_GROUP_2 { get; set; }

        public string IS_MISU_PTTT_GROUP_4 { get; set; }

        public string IS_MISU_PTTT_GROUP_3 { get; set; }

        public string IS_MISU_PTTT_GROUP_2 { get; set; }

        public string IS_MISU_PTTT_GROUP_1 { get; set; }

        public string TDL_SERVICE_NAMEs { get; set; }

        public string MANNERs { get; set; }

        public string RELATIVE_PHONE { get; set; }//điện thoại người thân

        public string RELATIVE_MOBILE { get; set; }//di động người thân
    }
    public class PTTT_INFO
    {
        public long? ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }//loại phẫu thuật
        public long? TDL_SERVICE_TYPE_ID { get; set; }//loại dịch vụ
        public string LOGINNAME { get; set; }//bác sĩ phẫu thuật
        public string USERNAME { get; set; }//bác sĩ phẫu thuật
        public long? PTTT_METHOD_ID { get; set; }//phương pháp phẫu thuật
        public long? EMOTIONLESS_METHOD_ID { get; set; } //phuong pháp gây mê (;ấy phương pháp 2 trong màn hình phẫu thuật)
        public string MANNER { get; set; }//cách thức phẫu thuật
        public string TDL_SERVICE_NAME { get; set; }//tên phẫu thuật
        public string RELATIVE_PHONE { get; set; }//điện thoại người thân
        public string RELATIVE_MOBILE { get; set; }//di động người thân
    }
}
