using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00387
{
    public class Mrs00387RDO : V_HIS_EXP_MEST_MEDICINE
    {
        public decimal MOBA_AMOUNT { get;  set;  }
        public decimal PRICE_VAT { get;  set;  }

        public Mrs00387RDO() { }

        public Mrs00387RDO(V_HIS_EXP_MEST_MEDICINE data)
        {
            try
            {
                if (data != null)
                {
                    System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_EXP_MEST_MEDICINE>(); 
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
