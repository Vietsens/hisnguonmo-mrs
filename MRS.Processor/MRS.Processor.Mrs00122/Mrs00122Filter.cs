using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Filter
{
    public class Mrs00122Filter : FilterBase
    {
        public long EXP_TIME_FROM { get; set; }
        public long EXP_TIME_TO { get; set; }
        public string EXP_LOGINNAME { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? IMP_MEDI_STOCK_ID { get; set; }
        public List<long> IMP_MEDI_STOCK_IDs { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? EXP_MEST_REASON_ID { get; set; }
        public List<long> EXP_MEST_REASON_IDs { get; set; }

        public long? DEPARTMENT_ID { get;  set;  }
        public string KEY_GROUP_EXP { get; set; }

        public Mrs00122Filter()
            : base()
        {

        }
    }
}
