using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00562
{
    public class Mrs00562Filter : HisHeinApprovalFilterQuery
    {
        public long? DEPARTMENT_ID { get; set; }
        public bool BLOOD_IS_SV { get; set; }
        public bool TRAN_IS_SV { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IS_ROUTE { get; set; }
    }
}
