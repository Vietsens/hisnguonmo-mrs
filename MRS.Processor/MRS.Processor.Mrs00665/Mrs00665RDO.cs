using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00665
{
    public class Mrs00665RDO
    {
        public V_HIS_TRANSACTION TRANSACTION { get; set; }
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public HIS_DEPARTMENT_TRAN HIS_DEPARTMENT_TRAN { get; set; }
        public string NOW_DEPARTMENT_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string DOB_YEAR { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public string DESCRIPTION { get;  set;  }
        public decimal? DEPOSIT_CASH { get;  set;  }
        public decimal? DEPOSIT_TRANSFER { get;  set;  }
        public decimal? DEPOSIT_OTHER_PAY_FORM { get;  set;  }
        public decimal? REPAY { get;  set;  }
        public string REPAY_USERNAME { get; set; }

        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
    }
}
