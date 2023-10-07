using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00304
{
    public class Mrs00304RDO : V_HIS_EXP_MEST
    {
        public long EXP_TIME { get; set; }
        public string EXP_TIME_STR { get; set; }
        //public string EXP_MEST_CODE { get;  set;  }
        public string PRESCRIPTION_CODE { get; set; }

        public string CLIENT_NAME { get; set; }
        //public string REQ_USERNAME { get;  set;  }
        public decimal PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal VAT_RATIO { get; set; }
        public decimal VIR_TOTAL_VAT_RATIO { get; set; }
        public decimal VIR_TOTAL_PRICE_ALL { get; set; }

        public string TRANSACTION_CODE { get; set; }
        public long? TRANSACTION_NUM_ORDER { get; set; }
    }
}
