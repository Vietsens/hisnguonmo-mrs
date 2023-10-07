using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00745
{
    class Mrs00745RDO : HIS_QC_NORMATION
    {
        public string QC_TYPE_CODE { get; set; }
        public string QC_TYPE_NAME { get; set; }
        public string MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string CREATOR { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public string USER_NAME { get; set; }
    }

    class MACHINE : LIS_MACHINE
    {
        public string MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }
        public string CREATOR { get; set; }
    }

    class DEPARTMENT : HIS_DEPARTMENT
    {
        public string ROOM_CODE { get; set; }
    }
    
}
