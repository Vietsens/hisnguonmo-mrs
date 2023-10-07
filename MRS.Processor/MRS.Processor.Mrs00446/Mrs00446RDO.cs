using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00446
{
    public class Mrs00446RDO
    {

        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public long IN_TIME { get;  set;  } 
        public long? OUT_TIME { get;  set;  }

        public decimal TOTAL_PRICE_XQUANG { get;  set;  } //445XQ
        public decimal TOTAL_PRICE_HOASINH { get;  set;  } //445HS
        public decimal TOTAL_PRICE_GIUONGDV { get;  set;  } //445GIUONGDV
        public decimal TOTAL_PRICE_GIUONG { get;  set;  }
        public decimal TOTAL_PRICE_VISINH { get;  set;  } //445VS
        public decimal TOTAL_PRICE_THUTHUATNHI { get;  set;  } //445TTNHI
        public decimal TOTAL_PRICE_CHIPHIKHAC { get;  set;  } //445CPK
        public decimal TOTAL_PRICE_DONGMAU { get;  set;  } //445DM
        public decimal TOTAL_PRICE_HUYETHOC { get;  set;  } //445HH
        public decimal TOTAL_PRICE_PTTH { get;  set;  } //445PTTH
        public decimal TOTAL_PRICE_PHAUTHUAT { get;  set;  }
        public decimal TOTAL_PRICE_THUTHUATNOI { get;  set;  } //445TTNOI
        public decimal TOTAL_PRICE_THUTHUAT { get;  set;  }
        public decimal TOTAL_PRICE_TRUYENMAU { get;  set;  } //445TRUYEN
        public decimal TOTAL_PRICE_THUTHUATNGOAI { get;  set;  } //445TTNGOAI
        public decimal TOTAL_PRICE_DIENTIM { get;  set;  } //445DT
        public decimal TOTAL_PRICE_SIEUAM { get;  set;  } //445SA
        public decimal TOTAL_PRICE_THUTHUATRHM { get;  set;  } //445RHM
        public decimal TOTAL_PRICE_GPB { get;  set;  } //445GPB
        public decimal TOTAL_PRICE_THUOC { get;  set;  }
        public decimal TOTAL_PRICE_MAU { get;  set;  }
        public decimal TOTAL_PRICE_VTTH { get;  set;  }
        public decimal TOTAL_PRICE_KHAM { get;  set;  }
        public decimal TOTAL_PRICE_ANHSIEUAM { get;  set;  } //445ASA
    }
}
