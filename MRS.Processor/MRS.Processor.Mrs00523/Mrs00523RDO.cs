using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00523
{
    public class Mrs00523RDO :V_HIS_EXP_MEST
    {

        public decimal COUNT_TREATMENT { get; set; }	
        public string EXP_DATE_STR { get; set; }


        public Mrs00523RDO(V_HIS_EXP_MEST r)
        {
            PropertyInfo[] p = typeof(V_HIS_EXP_MEST).GetProperties();
            
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            this.EXP_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.FINISH_TIME??0);
            this.COUNT_TREATMENT = 1;
        }

        public Mrs00523RDO()
        {
            // TODO: Complete member initialization
        }
    }
}
