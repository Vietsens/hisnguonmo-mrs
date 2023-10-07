using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00585
{
    public class Mrs00585Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public string CATEGORY_CODE__ECG { get; set; }
        public string CATEGORY_CODE__BRAIN_BLOOD { get; set; }
        public string CATEGORY_CODE__CERVICAL_ENDO { get; set; }

        public string CATEGORY_CODE__NT { get; set; }

        public string CATEGORY_CODE__HH { get; set; }

        public string CATEGORY_CODE__VS { get; set; }

        public string CATEGORY_CODE__SH { get; set; }


        public List<string> SERVICE_CODE__TRANs { get; set; }
    }
}