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

namespace MRS.Processor.Mrs00711
{
    public class Mrs00711RDO
    {
        public string TDL_PATIENT_DISTRICT_CODE { get; set; }
        public string TDL_PATIENT_DISTRICT_NAME { get; set; }
        public int COUNT_TREATMENT { get; set; }
        public int COUNT_TREATMENT_DTTT { get; set; }
        public int COUNT_TREATMENT_MO { get; set; }
        public int COUNT_TREATMENT_QU { get; set; }
        public int COUNT_TREATMENT_GL { get; set; }
        public decimal RATIO { get; set; }
    }
}
