using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00887
{
    class Mrs00887RDO : V_HIS_TREATMENT
    {
        public long MONTH { get; set; }

        public string MONTH_STR { get; set; }

        public string ICD_TYPE { get; set; }

        public long? ICD_GROUP_ID { get; set; }

        public string END_TYPE_NAME { get; set; }

        public string END_TYPE_CODE { get; set; }
    }
}
