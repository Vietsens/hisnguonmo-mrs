using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00273
{
    public class Mrs00273RDO : V_HIS_IMP_MEST_MEDICINE
    {
        public decimal? TOTAL_PRICE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

    }
   
}
