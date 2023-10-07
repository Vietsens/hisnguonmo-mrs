using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00677
{
    public class Mrs00677RDO
    {
        public string DEPARTMENT_NAME { get; set; }
        public long COUNT_PTTT_GROUP_ID_1 { get; set; }
        public long COUNT_PTTT_GROUP_ID_2 { get; set; }
        public long COUNT_PTTT_GROUP_ID_3 { get; set; }
        public long COUNT_PTTT_GROUP_ID_DB { get; set; }
        public long COUNT_PTTT_GROUP_ID_SUM { get; set; }
        public long COUNT_IN_TIME { get; set; }
        public long COUNT_OUT_TIME { get; set; }
        public long COUNT_TT_KCC { get; set; }
        public long COUNT_TT_KCC_PMO { get; set; }
        public long COUNT_TT_KCC_YC { get; set; }
        
    }
    public class SURGERY
    {
        public string DEPARTMENT_NAME { get; set; }
        public long PTTT_GROUP_ID { get; set; }
        public long PTTT_PRIORITY_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public short IS_IN_TIME { get; set; }
        public short IS_OUT_TIME { get; set; }
    }
}
