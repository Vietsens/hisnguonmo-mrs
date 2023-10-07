using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00281
{
    public class Mrs00281Filter
    {
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? BRANCH_ID { get; set; }
        public string LOGINNAME { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
    }
}
	