using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00880
{
    class Mrs00880RDO
    {
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public decimal AMOUNT { get; set; }
        public string TITLE { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string EMPLOYEE_ID_STR { get; set; }
        public long EMPLOYEE_ID { get; set; }
    }
}
