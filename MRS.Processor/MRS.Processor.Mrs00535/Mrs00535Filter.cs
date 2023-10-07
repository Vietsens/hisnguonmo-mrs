using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00535
{
    public class Mrs00535Filter : HisHeinApprovalFilterQuery
    {
        public long? DEPARTMENT_ID { get; set; }
        public bool BLOOD_IS_GENERIC_MEDI { get; set; }
        public bool BLOOD_IS_MEDI { get; set; }
        public bool TRAN_IS_MEDI { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IS_ROUTE { get; set; }
    }
}
