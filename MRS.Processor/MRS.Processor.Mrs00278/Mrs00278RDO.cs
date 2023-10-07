using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00278
{
    public class Mrs00278RDO : V_HIS_EXP_MEST_MEDICINE
    {
        public decimal? TOTAL_PRICE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set; }
        public decimal IMP_PRICE_BEFORE_VAT { get; set; }
    }
}
