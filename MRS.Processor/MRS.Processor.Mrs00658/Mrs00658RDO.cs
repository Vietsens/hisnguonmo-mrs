using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00658
{
    class Mrs00658RDO : V_HIS_EKIP_USER
    {
        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string BANK { get; set; }
        public string EMPLOYEE_DEPARTMENT_NAME { get; set; }
        public string DIPLOMA { get; set; }
        public short? IS_ADMIN { get; set; }
        public short? IS_DOCTOR { get; set; }
    }
}
