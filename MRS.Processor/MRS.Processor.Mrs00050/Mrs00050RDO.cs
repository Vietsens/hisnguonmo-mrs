using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00050
{
    class Mrs00050RDO : V_HIS_TRANSACTION
    {
        public long CANCEL_DATE { get; set; }
        public decimal TOTAL_BILL_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }
        public decimal TOTAL_REPAY_AMOUNT { get; set; }
        public decimal TOTAL_EXEMPTION { get; set; }

        public long? RE_TRANSACTION_TIME { get; set; }
        public string RE_EINVOICE_NUM_ORDER { get; set; }
        public decimal? RE_AMOUNT { get; set; }
        public string RE_CASHIER_LOGINNAME { get; set; }
        public string RE_CASHIER_USERNAME { get; set; }
    }
}
