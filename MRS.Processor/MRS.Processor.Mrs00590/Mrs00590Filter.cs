using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00590
{
    public class Mrs00590Filter : HisTreatmentFilterQuery
    {
        public List<string> ICD_CODE__HHTs { get; set; }
        public List<string> ICD_CODE__VPs { get; set; }
        public List<string> ICD_CODE__CUs { get; set; }
        public List<string> ICD_CODE__LAs { get; set; }
        public List<string> ICD_CODE__LTs { get; set; }
        public List<string> ICD_CODE__QBs { get; set; }
        public List<string> ICD_CODE__TDs { get; set; }
        public List<string> ICD_CODE__TCs { get; set; }
        public List<string> ICD_CODE__VGs { get; set; }
        public List<string> ICD_CODE__ADs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<string> DISTRICT_CODEs { get; set; }
    }
}
