using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Core; 

namespace MRS.Processor.Mrs00498
{
    public class Mrs00498RDO
    {
        public long PARENT_ID { get;  set;  }
        public string PARENT_NAME { get;  set;  }

        public V_HIS_TREATMENT TREATMENT { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public long DOB { get;  set;  }

        public V_HIS_TREATMENT_FEE TREATMENT_FEE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_HEIN_PRICE { get;  set;  }

        public decimal EXAM_PRICE { get;  set;  }     // tiền khám
        public decimal TEST_PRICE { get;  set;  }     // tiền xét nghiệm
        public decimal DIIM_PRICE { get;  set;  }     // tiền cđha
        public decimal MISU_PRICE { get;  set;  }     // tiền thủ thuật
        public decimal FUEX_PRICE { get;  set;  }     // tiền thăm dò chức năng
        public decimal MEDI_PRICE { get;  set;  }     // tiền thuốc
        public decimal MATE_PRICE { get;  set;  }     // tiền vật tư
        public decimal BED_PRICE { get;  set;  }      // tiền giường
        public decimal ENDO_PRICE { get;  set;  }     // tiền nội soi
        public decimal SUIM_PRICE { get;  set;  }     // tiền siêu âm
        public decimal SURG_PRICE { get;  set;  }     // tiền phẫu thuật
        public decimal OTHER_PRICE { get;  set;  }    // tiền dịch vụ khác
        public decimal PHCN_PRICE { get;  set;  }     // tiền phcn
        public decimal BLOOD_PRICE { get;  set;  }    // tiền máu
        public decimal GPBL_PRICE { get;  set;  }     // tiền giải phẫu bệnh lý

        public Mrs00498RDO() { }
    }
}
