using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00428
{
    public class Mrs00428RDO:V_HIS_EXP_MEST_BLOOD
    {
        public string REQ_DEPARTMENT_CODE { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string REQ_ROOM_CODE { get; set; }
        public string REQ_ROOM_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal AMOUNT_BH { get; set; }
        public decimal AMOUNT_VP { get; set; }
        public decimal AMOUNT_TRA { get; set; }
        public decimal AMOUNT_NHAP { get; set; }
        public decimal EXP_BLOOD_ABO_1 { get; set; }
        public decimal EXP_BLOOD_ABO_2 { get;  set;  }
        public decimal EXP_BLOOD_ABO_3 { get;  set;  }
        public decimal EXP_BLOOD_ABO_4 { get;  set;  }
        public decimal EXP_BLOOD_ABO_5 { get;  set;  }
        public decimal EXP_BLOOD_ABO_6 { get;  set;  }
        public decimal EXP_BLOOD_ABO_7 { get;  set;  }

        public decimal IMP_BLOOD_ABO_1 { get;  set;  }
        public decimal IMP_BLOOD_ABO_2 { get;  set;  }
        public decimal IMP_BLOOD_ABO_3 { get;  set;  }
        public decimal IMP_BLOOD_ABO_4 { get;  set;  }
        public decimal IMP_BLOOD_ABO_5 { get;  set;  }
        public decimal IMP_BLOOD_ABO_6 { get;  set;  }
        public decimal IMP_BLOOD_ABO_7 { get;  set;  }

        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }

        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }
    }

    public class BLOOD_ABO
    {
        public long ID { get;  set;  }
        public string BLOOD_ABO_CODE { get;  set;  }
        public string EXP_BLOOD_ABO_TAG { get;  set;  }
        public string IMP_BLOOD_ABO_TAG { get;  set;  }
    }
}
