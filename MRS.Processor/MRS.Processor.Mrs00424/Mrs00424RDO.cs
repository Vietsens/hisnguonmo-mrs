using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00424
{
    class Mrs00424RDO
    {
        public string EXP_MEST_TYPE_NAME { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public decimal? TOTAL_AMOUNT { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set; }
        public decimal? PRICE { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public decimal? TOTAL_PRICE { get;  set;  }
        public string CONCENTRA { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; } 
        public Mrs00424RDO() { }

        public string MATERIAL_TYPE_NAME { get; set; }

        public string MEDI_MATE_TYPE_NAME { get; set; }
    }
}
