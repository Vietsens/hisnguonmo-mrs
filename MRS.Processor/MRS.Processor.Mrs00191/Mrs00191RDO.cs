using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00191
{
 
public class Mrs00191RDO
    {
        public int STT { get;  set;  }
        public string OUT_ORDER { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string AGE_MEN { get;  set;  }
        public string AGE_WOMEN { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string CAREER { get;  set;  }
        public string END_DEPARTMENT_NAME { get;  set;  }
        public string OUT_TIME { get;  set;  }
        public string ICD_TEX { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string TRAN_PATI_FORM_CODE_1 { get;  set;  }
        public string TRAN_PATI_FORM_CODE_2 { get;  set;  }
        public string TRAN_PATI_FORM_CODE_3 { get;  set;  }
        public string TRAN_PATI_FORM_CODE_4 { get;  set;  }
        public string REASON_NAME_1 { get;  set;  }
        public string REASON_NAME_2 { get;  set;  }
        public string MEDI_ORG_NAME { get;  set;  }
        public string END_USERNAME { get;  set;  }


        public string ICD_CODE { get; set; }

        public string ICD_NAME_NEW { get; set; }
    }
}
