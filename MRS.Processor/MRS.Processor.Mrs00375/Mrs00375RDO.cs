using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00375
{
    public class Mrs00375RDO
    {
        public string PATIENT_NAME { get;  set;  }
        public string INTRUCTION_TIME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string INTRUCTION_TIME_FROM { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public long PATIENT_TYPE_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public string ICD_MAIN_TEXT { get;  set;  }
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }
        public long REPORT_TYPE_CAT_ID { get;  set;  }
        public string CATEGORY_NAME { get;  set;  }
        public string CATEGORY_CODE { get;  set;  }
        public char IS_BHYRT { get;  set;  }
        public string MALE_AGE { get;  set;  }
        public string FEMALE_AGE { get;  set;  }
        // CÁC NHÓM DỊCH VỤ
        public string TNTL_BECK { get;  set;  }
        public string TNTL_ZUNG { get;  set;  }
        public string TNTL_RAVEN { get;  set;  }
        public string TDGTC_HAMILTON { get;  set;  }
        public string TDGTC_TE { get;  set;  }
        public string TDGTN_WMS { get;  set;  }
        public string TDGNC_MMPI { get;  set;  }
        public string TNRLGN_PSQI { get;  set;  }
        public string TDGLOAU_HAMILTON { get;  set;  }
       // public string TDGNC_MMPI { get;  set;  }
        public string TDGSPTTE_DENVER { get;  set;  }
        public string TSLTKTE_CHAT { get;  set;  }
        public string TDGMDTK_CARS { get;  set;  }
        public string TDGHVTE_CBCL { get;  set;  }
        public string TN_WAIS { get;  set;  }
        public string TN_WICS { get;  set;  }
        public string BNKNCHN_EPI { get;  set;  }
        public string THANG_VANDERBILT { get;  set;  }
        public string TDGTCNG_GDS { get;  set;  }
        public string MMSE { get;  set;  }

        public string TDGTCSS_EPDS { get;  set;  }
        public string TDGLATCSTRESS_DASS { get;  set;  }
        public string TDGTC_YOUNG { get;  set;  }
        


    }
}
