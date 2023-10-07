using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00511
{
    public class Mrs00511RDO
    {
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }

        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public long DOB { get; set; }

        public decimal TNT_TCK { get; set; }
        public decimal TNT_NCK { get; set; }
        public decimal HDF_BH { get; set; }
        public decimal HDF_ND { get; set; }

        public decimal PRICE_TNT { get; set; }

        public Mrs00511RDO() { }
    }
}
