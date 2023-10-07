using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace MRS.Processor.Mrs00474
{
    public class REVENUE
    {
        public Decimal EXAM_PRICE { get;  set;  }//kham	
        public Decimal FUEX_PRICE { get;  set;  }//tham do chuc nang
        public Decimal ENDO_PRICE { get;  set;  }//noi soi
        public Decimal SUIM_PRICE { get;  set;  }//sieu am
        public Decimal OTHER_PRICE { get;  set;  }//khac
        public Decimal BED_PRICE { get;  set;  }//giuong
        public Decimal BLOOD_PRICE { get;  set;  }//mau
        public Decimal TEIN_PRICE { get;  set;  }//xet nghiem
        public Decimal DIIM_PRICE { get;  set;  }	//chup xquang
        public Decimal MISU_PRICE { get;  set;  }//thu thuat
        public Decimal SURG_PRICE { get;  set;  }	//phau thuat
        public Decimal MEDI_PRICE { get;  set;  }//thuoc
        public Decimal MATE_PRICE { get;  set;  }//vat tu
        public Decimal EXPEND_TICK { get;  set;  }
        public Decimal MEDI_EXPEND { get;  set;  }
        public Decimal MATE_EXPEND { get;  set;  }
        public Decimal CHEMICAL_EXPEND { get;  set;  }
    }
}
