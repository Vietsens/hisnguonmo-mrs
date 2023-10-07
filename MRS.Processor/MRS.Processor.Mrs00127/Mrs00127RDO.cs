using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00127
{
    class Mrs00127RDO:V_HIS_EXP_MEST_MEDICINE
    {

        public Mrs00127RDO(V_HIS_EXP_MEST_MEDICINE o)
        {
             try
            {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MEDICINE>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(o)));
                }
                SetExtendField(this);

            }
             catch (Exception ex)
             {

                 Inventec.Common.Logging.LogSystem.Error(ex);
             }
        }

        private void SetExtendField(Mrs00127RDO o)
        {
            try
            {
                EXP_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.EXP_DATE.ToString());
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        public string EXP_DATE_STR { get;  set;  }
        public decimal TOTAL_AMOUNT { get;  set;  }
        public decimal? TOTAL_DISCOUNT { get; set; }
        public decimal? TOTAL_PRICE { get; set; }

    }
}
