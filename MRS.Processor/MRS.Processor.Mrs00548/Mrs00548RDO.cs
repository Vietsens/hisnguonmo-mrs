using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00548
{
    public class Mrs00548RDO : V_HIS_TREATMENT
    {
        public string PATIENT_TYPE_NAME { get; set; }
        public string CREATOR_DEPO { get; set; }
        public string CASHIER_USERNAME_DEPO { get; set; }
        public string CREATOR_REPA { get; set; }
        public string CASHIER_USERNAME_REPA { get; set; }
        public decimal DEPOSIT_AMOUNT { get; set; }
        public decimal REPAY_AMOUNT { get; set; }
        public long? REPAY_TIME { get; set; }
    }
}
