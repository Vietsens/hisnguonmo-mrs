using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00740
{
    class Mrs00740RDO : V_HIS_IMP_MEST_BLOOD
    {
        public long EXECUTE_TIME { get; set; }
        public string IMP_MEST_TYPE_NAME { get; set; }

        public string BLOOD_GROUP { get; set; }

        public string EXECUTE_TIME_STR { get; set; }

        public string EXPIRED_DATE_STR { get; set; }

        public int DAY_LEFT { get; set; }

        public string IMP_TIME_STR { get; set; }
    }
}
