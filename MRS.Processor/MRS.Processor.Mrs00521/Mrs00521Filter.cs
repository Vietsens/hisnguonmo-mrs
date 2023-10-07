using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00521
{
    public class Mrs00521Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<string> ICD_CODE__GYNECOLOGICALs { get; set; }
        public List<string> ICD_CODE__PROCREATE_COMMONs { get; set; }
        public List<string> ICD_CODE__PROCREATE_DIFFICULTs { get; set; }
        public List<string> ICD_CODE__PROCREATE_SURGs { get; set; }
        public List<string> ICD_CODE__GYNECOLOGICAL_SURGs { get; set; }
        public List<string> SERVICE_CODE__ABORTIONs { get; set; }

        public bool? IS_AMOUNT_TREATMENT { get; set; }
    }
}
