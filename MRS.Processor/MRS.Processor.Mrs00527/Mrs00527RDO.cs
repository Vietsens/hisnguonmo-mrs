using MRS.Processor.Mrs00527;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;

namespace MRS.Proccessor.Mrs00527
{
    public class Mrs00527RDO : HIS_TREATMENT
    {

        public string AMOUNT_STR { get; set; }
        public long SERVICE_ID { get; set; }
        public decimal AMOUNT { get; set; }

        public Mrs00527RDO(HIS_SERE_SERV r, List<HIS_TREATMENT> listHisTreatment)
        {
            var ss = listHisTreatment.FirstOrDefault(o => r.TDL_TREATMENT_ID == o.ID) ?? new HIS_TREATMENT();
			PropertyInfo[] p = typeof(HIS_TREATMENT).GetProperties();
			foreach	(var item in p)
			{
                item.SetValue(this, item.GetValue(ss));
			}

            this.AMOUNT= r.AMOUNT;
            this.SERVICE_ID = r.SERVICE_ID;
        }
		
		public Mrs00527RDO()
        {

        }
    }
}
