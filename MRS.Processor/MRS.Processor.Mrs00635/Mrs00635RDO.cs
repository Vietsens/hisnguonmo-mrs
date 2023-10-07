using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00635
{
    public class Mrs00635RDO
    {
        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }

        public long SERVICE_ID { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }

        public decimal? AMOUNT { get; set; }

        public decimal? VIR_PRICE { get; set; }

        public decimal? VIR_TOTAL_PRICE { get; set; }

        public long TDL_INTRUCTION_DATE { get; set; }


        public string PTTT_GROUP_NAME{ get; set; }

        public string TREATMENT_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string TDL_REQUEST_USERNAME { get; set; }
    }
}
