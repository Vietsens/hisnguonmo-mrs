using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00558
{
    public class Mrs00558Filter : HisHeinApprovalFilterQuery
    {
        public long? DEPARTMENT_ID { get; set; }
        public bool BLOOD_IS_MATE { get; set; }
        public bool TRAN_IS_MATE { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IS_ROUTE { get; set; }
    }
}
