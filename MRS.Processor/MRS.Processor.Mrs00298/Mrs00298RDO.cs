using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00298
{
    public class Mrs00298RDO : V_HIS_TREATMENT_FEE
    {
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string HEIN_CARD_NUMBER_STR { get; set; }
        public string DEPARTMENT { get; set; }
        public string DOB_YEAR { get; set; }
        public Mrs00298RDO(V_HIS_TREATMENT_FEE data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TREATMENT_FEE>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }
                this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.IN_TIME);
                this.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.OUT_TIME ?? 0);
                data.TDL_PATIENT_DOB = System.DateTime.Now.Year - (int)(data.TDL_PATIENT_DOB / 10000000000);
                this.DOB_YEAR = data.TDL_PATIENT_DOB.ToString();
                this.DEPARTMENT = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == data.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                this.HEIN_CARD_NUMBER_STR = data.TDL_HEIN_CARD_NUMBER;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
