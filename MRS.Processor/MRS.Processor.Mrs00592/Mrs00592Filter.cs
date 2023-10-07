using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00592
{
    public class Mrs00592Filter : HisTreatmentFilterQuery
    {
        
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<string> ICD_CODE__PROCREATE_COMMONs { get; set; }
        public List<string> ICD_CODE__PROCREATE_DIFFICULTs { get; set; }
        public List<string> ICD_CODE__PROCREATE_EARLYs { get; set; }
        public List<string> ICD_CODE__PROCREATE_FX_GHs { get; set; }
        public List<string> ICD_CODE__PROCREATE_SURGs { get; set; }
    }
}
