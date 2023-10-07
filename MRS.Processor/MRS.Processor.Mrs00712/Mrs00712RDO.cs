using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00712
{
    public class Mrs00712RDO
    {
        public long PTTT_GROUP_ID { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }
    }
}
