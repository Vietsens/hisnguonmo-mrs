using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00012
{
    class PatientTypeRDO
    {
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
    }

    class DepartmentRDO
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public decimal AMOUNT { get; set; }
    }

    class BillRDO
    {
        public long? TRANSACTION_ID { get; set; }
        public long FEE_LOCK_TIME { get; set; } //thời gian khóa viện phí
        public long? OUT_TIME { get; set; } //thời gian ra viện
        public long? TRANSACTION_TIME { get; set; }
        public short SIGN { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string ACCOUNT_BOOK_NAME { get;  set;  }	//ten sổ thu
        public string TREATMENT_CODE { get; set; }	//mã điều trị
        public string TDL_PATIENT_CODE { get; set; }	//mã bệnh nhân
        public string TDL_PATIENT_NAME { get; set; }	//tên bệnh nhân
        public string DEPARTMENT_NAME { get; set; }	//khoa kết thúc
        public long? NUM_ORDER { get; set; }	//số biên lai
        public string EINVOICE_NUM_ORDER { get; set; }	//so hoa don dien tu
        public string BANK_TRANSACTION_CODE { get; set; }	//ma giao dich ngan hang
        public long? BANK_TRANSACTION_TIME { get; set; }	//thoi gian giao dich ngan hang
        public string TRANSACTION_CODE { get; set; } //ma giao dich
        public string PARENT_KEY { get; set; }//nguoi khoa vien phi
        public string LOGINNAME { get; set; }//nguoi khoa vien phi
        public string USERNAME { get; set; }//nguoi khoa vien phi
        public string KI_HIEU { get; set; } //ky hieu va mau so thu chi
        public decimal HAOPHI { get; set; } // hao phi
        public decimal AMOUNT_BH { get; set; } // so tien bn tra cho bao hiem
        public decimal AMOUNT_VP { get; set; } // so tien bn tra cho vien phi
        public decimal AMOUNT_YC { get; set; } // so tien bn tra cho yeu cau
        public decimal AMOUNT_DVYC { get; set; } // so tien bn tra cho dich vu yeu cau
        public decimal BHTT { get; set; } // so tien bao hiem thanh toan
        public decimal TAMUNG { get; set; } // so tien tam ung
        public decimal HOANUNG { get; set; } // so tien hoan ung
        public decimal HOANUNGTRUOC { get; set; } // so tien hoan ung truoc
        public decimal AMOUNT { get; set; } // so tien thanh toan
        public decimal KC_AMOUNT { get; set; } // so tien ket chuyen
        public decimal TDL_BILL_FUND_AMOUNT { get; set; } // so tien quy thanh toan
        public decimal MIENGIAM { get; set; } // so tien mien giam

        public decimal VIENPHI { get; set; }

        public long? PAY_FORM_ID { get; set; }
        public string PAY_FORM_NAME { get; set; }

        public long? ACCOUNT_BOOK_ID { get; set; }

        public BillRDO()
        {
            //
        }

    }

}
