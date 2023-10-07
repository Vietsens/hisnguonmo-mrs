using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00045
{
    class Mrs00045RDO : HIS_TREATMENT
    {

        public Mrs00045RDO(MOS.EFMODEL.DataModels.HIS_TREATMENT treatment)
        {
            if (treatment != null)
            {
                System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.HIS_TREATMENT>();
                foreach (var item in pis)
                {
                    item.SetValue(this, item.GetValue(treatment));
                }
            }
        }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string NUMBER_DEAD { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string IS_OFFICER { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_CITY { get;  set;  }
        public string IS_COUNTRYSIDE { get;  set;  }
        public string IS_DUOI_12THANG { get;  set;  }
        public string IS_1DEN15TUOI { get;  set;  }
        public string JOB { get;  set;  }
        public string GIOITHIEU { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DATE_DEAD_STR { get;  set;  }
        public string IS_DEAD_IN_24H { get;  set;  }
        public string DIAGNOSE_TUYENDUOI { get;  set;  }
        public string ICD_CODE_TUYENDUOI { get;  set;  }
        public string DIAGNOSE_KKB { get;  set;  }
        public string ICD_CODE_KKB { get;  set;  }
        public string DIAGNOSE_KDT { get;  set;  }
        public string ICD_CODE_KDT { get;  set;  }
        public string DIAGNOSE_KGBP { get;  set;  }
        public string ICD_CODE_KGBP { get;  set;  }
        public string NOTE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }

        public string PATIENT_DOB { get; set; }

        public string DEATH_CERT_BOOK_NAME { get; set; }

        public string DEATH_TIME_STR { get; set; }

        public string DEATH_CAUSE { get; set; }

        public string DEATH_TIMING { get; set; }

        public string IS_EXAMINATION { get; set; }

        public string DEATH_MAIN_CAUSE { get; set; }

        public string END_DEPARTMENT_NAME { get; set; }

        public string STORE_CREATOR { get; set; }
    }
}
