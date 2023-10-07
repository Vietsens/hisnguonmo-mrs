using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00388
{
    public class Mrs00388RDO: V_HIS_EXP_MEST_MATERIAL
    {
        public decimal MOBA_AMOUNT { get;  set;  }
        public decimal PRICE_VAT { get;  set;  }

        public Mrs00388RDO() { }

        public Mrs00388RDO(V_HIS_EXP_MEST_MATERIAL data)
        {
            try
            {
                if (data != null)
                {
                    System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_EXP_MEST_MATERIAL>(); 
                    foreach (var item in pi)
                    {
                        item.SetValue(this, item.GetValue(data)); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
