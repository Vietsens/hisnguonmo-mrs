using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00513
{
    public class Mrs00513RDO
    {
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public string BED_NAME { get; set; }

        public string PATIENT_NAME { get; set; }
        public long DOB { get; set; }

        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal AMOUNT { get; set; }

        public Mrs00513RDO() { }
    }
}
