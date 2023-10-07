using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00654
{
    class Mrs00654RDO : MOS.EFMODEL.DataModels.V_HIS_TREATMENT
    {
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string DOB_STR { get; set; }
        public string SICK_LEAVE_FROM_STR { get; set; }
        public string SICK_LEAVE_TO_STR { get; set; }
        public string FATHER_NAME { get; set; }
        public string MOTHER_NAME { get; set; }

        public long CURRENT_NUM_ORDER_PRINT { get; set; }
    }
}
