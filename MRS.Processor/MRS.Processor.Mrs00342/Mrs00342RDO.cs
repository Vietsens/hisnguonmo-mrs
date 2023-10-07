using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00342
{
    public class Mrs00342RDO
    {
        public long IMP_EXP_TIME { get;  set;  }
        public string IMP_EXP_CODE { get;  set;  }
        public string IMP_EXP_TYPE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal? IMP_AMOUNT { get;  set;  }
        public decimal? TOTAL_IMP_PRICE { get;  set;  }
        public decimal? EXP_AMOUNT { get;  set;  }
        public decimal? TOTAL_EXP_PRICE { get;  set;  }
        public decimal IMP_VAT_RATIO { get;  set;  }
        public decimal VAT_RATIO { get;  set;  }


        public string CLIENT_NAME { get;  set;  }         // người mua (SALE)
        public string VIR_PATIENT_NAME { get;  set;  }    // tên bệnh nhân (PRES)
        public string TREATMENT_CODE { get;  set;  }      // mã điều trị (PRES)
        public string MEDI_STOCK_NAME { get;  set;  }     // kho xuất nhập (CHMS)
        public string DEPARTMENT_NAME { get;  set;  }     // khoa nhập, xuất (DEPA, MOBA)
        public string ROOM_NAME { get;  set;  }           // phòng nhập / xuất (DEPA, MOBA)
        public string MOBA_EXP_MEST_CODE { get;  set;  }  // phiếu xuất bị thu hồi

        public Mrs00342RDO()
        {

        }
    }

    public class SERVICE
    {
        public string MEDI_STOCK_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
    }
}

