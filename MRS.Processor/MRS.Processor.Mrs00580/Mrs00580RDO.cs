using MRS.Processor.Mrs00580;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00580
{
    public class Mrs00580RDO
    {
        public long? EXP_TIME { get; set; }	
        public string EXP_TIME_STR { get; set; }	
        public string EXP_MEST_CODE { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }

        public string JSON_AMOUNT { get; set; }
        public Dictionary<string,decimal> DIC_AMOUNT { get; set; }
        public Mrs00580RDO()
        {
            DIC_AMOUNT = new Dictionary<string, decimal>();
        }

        public long? AGGR_EXP_MEST_ID { get; set; }
    }
}
