using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00159
{
    class Mrs00159RDO
    {
        public long PARENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DOB { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string HEIN_MEDI_ORG_CODE { get;  set;  }
        //ngay kham - select*from v_his_service_req
        public string ICD_CODE { get;  set;  }
        //so phieu thanh toan ra vien
        //so TT duyet KT
        //so tien benh nhan phai thanh toan
        public decimal? AMOUNT_SERV { get;  set;  }
        //so tien da thu tu benh nhan
        public decimal? VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public decimal AMOUNT_BILL { get;  set;  }
        public decimal AMOUNT_DEPOSIT { get;  set;  }
        public decimal AMOUNT_REPAY { get;  set;  }
        //public decimal? AMOUNT_BHYT { get;  set;  }
        public decimal XN { get;  set;  }
        public decimal CDHA { get;  set;  }
        public decimal THUOC { get;  set;  }
        public decimal PTTT { get;  set;  }
        public decimal MAU { get;  set;  }
        public decimal VTYT { get;  set;  }
        public decimal DVKTC { get;  set;  }
        public decimal THUOC_K { get;  set;  }
        public decimal TIEN_KHAM { get;  set;  }
        public decimal TIEN_GIUONG { get;  set;  }
        public decimal CPVC { get;  set;  }
        public decimal TONG_CONG { get;  set;  }
        public decimal MIEN { get;  set;  }
        public string CREAT_TIME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
    }

    public class V_HIS_TREATMENT_NEW
    {
        public long DEPARTMENT_ID { get;  set;  }

        public V_HIS_TREATMENT V_HIS_TREATMENT { get;  set;  }
    }

    public class TOTAL_DEPARTMENT
    {
        public long DEPARTMENT_ID { get;  set;  }
        public decimal XN { get;  set;  }
        public decimal CDHA { get;  set;  }
        public decimal THUOC { get;  set;  }
        public decimal PTTT { get;  set;  }
        public decimal MAU { get;  set;  }
        public decimal VTYT { get;  set;  }
        public decimal DVKTC { get;  set;  }
        public decimal THUOC_K { get;  set;  }
        public decimal TIEN_KHAM { get;  set;  }
        public decimal TIEN_GIUONG { get;  set;  }
        public decimal CPVC { get;  set;  }
        public decimal TONG_CONG { get;  set;  }
        public decimal MIEN { get;  set;  }
        public decimal? AMOUNT_SERV { get;  set;  }
        //so tien da thu tu benh nhan
        public decimal? VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public decimal AMOUNT_BILL { get;  set;  }
        public decimal AMOUNT_DEPOSIT { get;  set;  }
        public decimal AMOUNT_REPAY { get;  set;  }
        
    }
}
