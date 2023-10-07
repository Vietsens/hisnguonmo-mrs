using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00430
{
    public class Mrs00430RDO
    {
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }

        public decimal TOTAL_BILL_AMOUNT { get; set; }
        public decimal TOTAL_BILL_FEE { get; set; }
        public decimal TOTAL_BILL_HEIN { get; set; }
        public decimal TOTAL_DEPOSITS_AMOUNT { get; set; }
        public decimal TOTAL_REPAYS_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_FEE { get; set; }
        public decimal TOTAL_DEPOSIT_HEIN { get; set; }
        public decimal TOTAL_REPAY_AMOUNT { get; set; }
        public decimal TOTAL_REPAY_HEIN { get; set; }
        public decimal TOTAL_REPAY_FEE { get; set; }
        public decimal TOTAL_EXEMPTION { get; set; }
    }
}
