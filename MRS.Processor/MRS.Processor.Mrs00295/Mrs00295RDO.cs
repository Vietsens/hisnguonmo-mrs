using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisService;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00295
{
    public class Mrs00295RDO
    {
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT_REPAY { get; set; }
        public decimal TOTAL_REPAY_PRICE { get; set; }
        
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long TREATMENT_ID { get; set; }

        public decimal AMOUNT_DEPOSIT { get; set; }
        public decimal TOTAL_DEPOSIT_PRICE { get; set; }

        public Mrs00295RDO() { }

    }
}
