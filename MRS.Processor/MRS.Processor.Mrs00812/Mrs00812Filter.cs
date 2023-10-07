using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00812
{
    public class Mrs00812Filter
    {
        public long TIME_FROM { get; set; }
        public long  TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { set; get; }
        public short? CHECK_TYPE_HEIN_CARD { set; get; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> TRANSACTION_TYPE_IDs { set; get; }
        public List<long> PAY_FORM_IDs { set; get; }
    }
}
