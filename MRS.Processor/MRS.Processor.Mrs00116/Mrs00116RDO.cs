using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00116
{
    class Mrs00116RDO
    {
        private MOS.EFMODEL.DataModels.V_HIS_DEPARTMENT_TRAN lstSereServ; 

        public string DEPARTMENT_NAME { get;  set;  }

        public int? TOTAL_PATIENT_DATE_1 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_2 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_3 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_4 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_5 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_6 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_7 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_8 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_9 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_10 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_11 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_12 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_13 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_14 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_15 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_16 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_17 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_18 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_19 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_20 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_21 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_22 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_23 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_24 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_25 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_26 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_27 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_28 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_29 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_30 { get;  set;  }
        public int? TOTAL_PATIENT_DATE_31 { get;  set;  }

        public Mrs00116RDO(List<string> lisString, string department)
        {
            DEPARTMENT_NAME = department; 
            TOTAL_PATIENT_DATE_1 = int.Parse(lisString[0]) != 0 ? (int?)int.Parse(lisString[0]) : null; 
            TOTAL_PATIENT_DATE_2 = int.Parse(lisString[1]) != 0 ? (int?)int.Parse(lisString[1]) : null; 
            TOTAL_PATIENT_DATE_3 = int.Parse(lisString[2]) != 0 ? (int?)int.Parse(lisString[2]) : null; 
            TOTAL_PATIENT_DATE_4 = int.Parse(lisString[3]) != 0 ? (int?)int.Parse(lisString[3]) : null; 
            TOTAL_PATIENT_DATE_5 = int.Parse(lisString[4]) != 0 ? (int?)int.Parse(lisString[4]) : null; 
            TOTAL_PATIENT_DATE_6 = int.Parse(lisString[5]) != 0 ? (int?)int.Parse(lisString[5]) : null; 
            TOTAL_PATIENT_DATE_7 = int.Parse(lisString[6]) != 0 ? (int?)int.Parse(lisString[6]) : null; 
            TOTAL_PATIENT_DATE_8 = int.Parse(lisString[7]) != 0 ? (int?)int.Parse(lisString[7]) : null; 
            TOTAL_PATIENT_DATE_9 = int.Parse(lisString[8]) != 0 ? (int?)int.Parse(lisString[8]) : null; 
            TOTAL_PATIENT_DATE_10 = int.Parse(lisString[9]) != 0 ? (int?)int.Parse(lisString[9]) : null; 
            TOTAL_PATIENT_DATE_11 = int.Parse(lisString[10]) != 0 ? (int?)int.Parse(lisString[10]) : null; 
            TOTAL_PATIENT_DATE_12 = int.Parse(lisString[11]) != 0 ? (int?)int.Parse(lisString[11]) : null; 
            TOTAL_PATIENT_DATE_13 = int.Parse(lisString[12]) != 0 ? (int?)int.Parse(lisString[12]) : null; 
            TOTAL_PATIENT_DATE_14 = int.Parse(lisString[13]) != 0 ? (int?)int.Parse(lisString[13]) : null; 
            TOTAL_PATIENT_DATE_15 = int.Parse(lisString[14]) != 0 ? (int?)int.Parse(lisString[14]) : null; 
            TOTAL_PATIENT_DATE_16 = int.Parse(lisString[15]) != 0 ? (int?)int.Parse(lisString[15]) : null; 
            TOTAL_PATIENT_DATE_17 = int.Parse(lisString[16]) != 0 ? (int?)int.Parse(lisString[16]) : null; 
            TOTAL_PATIENT_DATE_18 = int.Parse(lisString[17]) != 0 ? (int?)int.Parse(lisString[17]) : null; 
            TOTAL_PATIENT_DATE_19 = int.Parse(lisString[18]) != 0 ? (int?)int.Parse(lisString[18]) : null; 
            TOTAL_PATIENT_DATE_20 = int.Parse(lisString[19]) != 0 ? (int?)int.Parse(lisString[19]) : null; 
            TOTAL_PATIENT_DATE_21 = int.Parse(lisString[20]) != 0 ? (int?)int.Parse(lisString[20]) : null; 
            TOTAL_PATIENT_DATE_22 = int.Parse(lisString[21]) != 0 ? (int?)int.Parse(lisString[21]) : null; 
            TOTAL_PATIENT_DATE_23 = int.Parse(lisString[22]) != 0 ? (int?)int.Parse(lisString[22]) : null; 
            TOTAL_PATIENT_DATE_24 = int.Parse(lisString[23]) != 0 ? (int?)int.Parse(lisString[23]) : null; 
            TOTAL_PATIENT_DATE_25 = int.Parse(lisString[24]) != 0 ? (int?)int.Parse(lisString[24]) : null; 
            TOTAL_PATIENT_DATE_26 = int.Parse(lisString[25]) != 0 ? (int?)int.Parse(lisString[25]) : null; 
            TOTAL_PATIENT_DATE_27 = int.Parse(lisString[26]) != 0 ? (int?)int.Parse(lisString[26]) : null; 
            TOTAL_PATIENT_DATE_28 = int.Parse(lisString[27]) != 0 ? (int?)int.Parse(lisString[27]) : null; 
            TOTAL_PATIENT_DATE_29 = int.Parse(lisString[28]) != 0 ? (int?)int.Parse(lisString[28]) : null; 
            TOTAL_PATIENT_DATE_30 = int.Parse(lisString[29]) != 0 ? (int?)int.Parse(lisString[29]) : null; 
            TOTAL_PATIENT_DATE_31 = int.Parse(lisString[30]) != 0 ? (int?)int.Parse(lisString[30]) : null; 
        }
    }
}
