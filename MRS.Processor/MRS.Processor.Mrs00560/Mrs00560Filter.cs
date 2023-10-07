using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00560
{
    public class Mrs00560Filter : HisHeinApprovalFilterQuery
    {
        public long? DEPARTMENT_ID { get; set; }
        public bool BLOOD_IS_GENERIC_MEDI { get; set; }
        public bool BLOOD_IS_MEDI { get; set; }
        public bool TRAN_IS_MEDI { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}
