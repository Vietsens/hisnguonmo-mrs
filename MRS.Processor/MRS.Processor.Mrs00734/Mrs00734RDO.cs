using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00734
{
    class Mrs00734RDO
    {
        public string LOGIN_NAME { get; set; }
        public string USER_NAME { get; set; }
        public long LOGIN_TIME { get; set; }
        public long ACTIVITY_TYPE_ID { get; set; }
        public string ACTIVITY_TYPE_CODE { get; set; }
        public string ACTIVITY_TYPE_NAME { get; set; }
        public long ACTIVITY_TIME { get; set; }
        public long WORKING_TIME { get; set; }
        public string WORKING_TIME_STR { get; set; }
        public long LATEST_LOGIN_TIME { get; set; }
        public string LATEST_LOGIN_TIME_STR { get; set; }

        public long LOGOUT_TIME { get; set; }

        public string LOGOUT_TIME_STR { get; set; }
    }
}
