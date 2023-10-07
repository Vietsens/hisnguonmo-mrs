using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00448
{
    class Mrs00448RDO
    {
        public string CASHIER_USERID { get;  set;  } //nguoi thu
        public string CASHIER_USERNAME { get;  set;  } //nguoi thu
        public decimal TOTAL_TREAT_IN { get;  set;  } //noi tru
        public decimal TOTAL_TREAT_OUT { get;  set;  } // ngoai tru
        public decimal TOTAL_TREAT_IN_OUT { get;  set;  } // BN noitru ngoaitru vp
        public long TOTAL_NDTRI { get;  set;  } // ngay dieu tri noi tru vp
        public decimal TOTAL_EXAM { get;  set;  }
        public decimal TOTAL_EXAM_PHY { get;  set;  } //kham suc khoe
        public decimal TOTAL_BED { get;  set;  } //giuong
        public decimal TOTAL_BED_ALLO { get;  set;  } //phu cap giuong
        public decimal TOTAL_TEST { get;  set;  } //xet nghiem
        public decimal TOTAL_XQUANG { get;  set;  } //xquang
        public decimal TOTAL_XQUANG_CC { get;  set;  } //xquang can chop
        public decimal TOTAL_CT { get;  set;  } //ct
        public decimal TOTAL_SUIM { get;  set;  } //siêu am
        public decimal TOTAL_EEG { get;  set;  } //dien nao
        public decimal TOTAL_ECG { get;  set;  } //dien tim
        public decimal TOTAL_ENDO { get;  set;  } //noi soi
        public decimal TOTAL_ABOR { get;  set;  } //Hut thai
        public decimal TOTAL_MISU { get;  set;  } //thu thuat
        public decimal TOTAL_MISU_ALLO { get;  set;  } // pcap thu thuat
        public decimal TOTAL_INJE { get;  set;  } //tiem
        public decimal TOTAL_MIDI { get;  set;  } //thuoc
        public decimal TOTAL_MATE { get;  set;  } //Vat tu
        public decimal TOTAL_HCG { get;  set;  } //hcg
        public decimal TOTAL_HIV { get;  set;  } //hiv 
        public decimal TOTAL_APHE { get;  set;  } //Aphetamin
        public decimal TOTAL_MERE { get;  set;  }//sao benh an
        public decimal TOTAL_TOTAL_PRICE_FEE { get;  set;  } // tong tien
        public decimal TOTAL_HEIN_RATIO_5 { get;  set;  } // bao hiem 5
        public decimal TOTAL_HEIN_RATIO_20 { get;  set;  } // bao hiem 20
        public decimal TOTAL_EXEM { get;  set;  } // miem giam
        public decimal TOTAL_TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_LDLK{ get;  set;  }
        public Mrs00448RDO() { }
    }
    public class MY_PATIENT_TYPE_ALTER
    {
        public long ID { get;  set;  }
        public long TREATMENT_TYPE_ID { get;  set;  }
        public long LOG_IN_TIME { get;  set;  }
        public long LOG_OUT_TIME { get;  set;  }
    }
}
