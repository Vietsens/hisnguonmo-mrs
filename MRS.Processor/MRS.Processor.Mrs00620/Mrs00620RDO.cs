using MRS.Processor.Mrs00620;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Logging;

namespace MRS.Proccessor.Mrs00620
{
    public class Mrs00620Service
    {
        public long DEPARTMENT_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public int COUNT_TREAT { get; set; }
        public decimal PREVIOUS_AMOUNT { get; set; }
        public decimal CURENT_AMOUNT { get; set; }
    }

    public class AMOUNT_INFO
    {
        public decimal COUNT_BEGIN { get; set; }
        public decimal COUNT_IMP { get; set; }

        public decimal COUNT_EXP{ get; set; }
        public decimal COUNT_END { get; set; }

        public decimal COUNT_EXP_BHYT { get; set; }
        public decimal COUNT_EXP_KHOI { get; set; }
        public decimal COUNT_EXP_DO { get; set; }
        public decimal COUNT_EXP_KTD { get; set; }
        public decimal COUNT_EXP_NANG { get; set; }
        public decimal COUNT_EXP_CV { get; set; }

        public decimal AMOUNT_TEST { get; set; }
        public decimal AMOUNT_CDHA { get; set; }
        public decimal AMOUNT_PTTT { get; set; }
    }

    public class Mrs00620RDO
    {
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int NUM_BED { get; set; }
        public decimal DAY { get; set; }
        public decimal DTTB { get; set; }
        public decimal SDGB { get; set; }
        public AMOUNT_INFO PREVIOUS_INFO { get; set; }
        public AMOUNT_INFO CURENT_INFO { get; set; }

        public Mrs00620RDO()
        {
            PREVIOUS_INFO = new AMOUNT_INFO();
            CURENT_INFO = new AMOUNT_INFO();
        }
    }
}
