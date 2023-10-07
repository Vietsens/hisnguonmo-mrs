using MRS.Processor.Mrs00598;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00598
{
    public class Mrs00598RDO
    {
        public long? IMP_TIME { get; set; }	
        public string IMP_TIME_STR { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string IMP_MEST_TYPE_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string AGGR_EXP_MEST_CODE { get; set; }

        public string JSON_AMOUNT { get; set; }
        public Dictionary<string,decimal> DIC_AMOUNT { get; set; }
        public Mrs00598RDO()
        {
            DIC_AMOUNT = new Dictionary<string, decimal>();
        }

        public long? AGGR_IMP_MEST_ID { get; set; }
    }
}
