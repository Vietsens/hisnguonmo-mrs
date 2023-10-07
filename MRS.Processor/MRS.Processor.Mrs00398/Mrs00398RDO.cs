using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00398
{
    public class Mrs00398RDO : HIS_TREATMENT
    {
        public long OUT_DEPARTMENT_ID { get; set; }

        public string OUT_DEPARTMENT_CODE { get; set; }
        public string OUT_DEPARTMENT_NAME { get; set; }
        public string OUT_ROOM_CODE { get; set; }
        public string OUT_ROOM_NAME { get; set; }

        //public long IN_TIME { get; set; }
        //public long OUT_TIME { get; set; }
        public long TREATMENT_ID { get;  set;  }
        //public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_DUOI_12THANG { get;  set;  }
        public string IS_1DEN15TUOI { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string GIOITHIEU { get; set; }
        public string DATE_IN_STR { get; set; }
        public string DATE_OUT_STR { get; set; }
        public string IS_DEAD_IN_24H { get;  set;  }
        public string IS_CURED { get;  set;  }
        public string IS_REDUCE { get;  set;  }
        public string IS_DIE { get; set; }
        public string IS_HEAVIER { get;  set;  }
        public string IS_CONSTANT { get;  set;  }
        public string DATE_DEAD_STR { get;  set;  }
        public string END_ORDER { get;  set;  }
        public decimal TOTAL_DATE_TREATMENT { get;  set;  }
        public string ICD_MAIN_TEXT { get; set; }


        public string RAVIEN_DT { get; set; }
        public string RAVIEN_TT { get; set; }
        public string RAVIEN_NAM_60 { get; set; }
        public string RAVIEN_NU_60 { get; set; }
        public string RAVIEN_15 { get; set; }
        public long? CV_TIME { get; set; }

        public string FEE_LOCK_DATE_STR { get;  set;  }

        public int MALE_AGE { get;  set;  }

        public int FEMALE_AGE { get; set; }

        public string SICK_LEAVE_FROM_STR { get; set; }
        public string SICK_LEAVE_TO_STR { get; set; }
        public string DIPLOMA { get; set; }

        public string FATHER_NAME { get; set; }

        public string MOTHER_NAME { get; set; }

        public string TREATMENT_END_TYPE_CODE { get; set; }

        public string TREATMENT_END_TYPE_NAME { get; set; }

        //thông tin liên quan đến tử vong

        public string PATIENT_CLASSIFY_CODE { get; set; }

        public Mrs00398RDO(HIS_TREATMENT r)
        {
            PropertyInfo[] p = typeof(HIS_TREATMENT).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
                SICK_LEAVE_FROM_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.SICK_LEAVE_FROM ?? 0);
                SICK_LEAVE_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.SICK_LEAVE_TO ?? 0);
            }
        }

        public Mrs00398RDO()
        {
           
        }


        public string WORK_PLACE_CODE { get; set; }

        public string HEIN_MEDI_ORG_CODE { get; set; }

        public string END_ADDRESS { get; set; }

        public string DOB_STR { get; set; }
        public decimal? CT { get; set; }
        public decimal? XQ { get; set; }
        public decimal? MRI { get; set; }

        public string TREATMENT_RESULT_CODE { get; set; }

        public string TREATMENT_RESULT_NAME { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }
    }
    public class TREATMENT_NUMFILM
    {
        public long ID { get; set; }
        public decimal? CT { get; set; }
        public decimal? XQ { get; set; }
        public decimal? MRI { get; set; }
    }
}
