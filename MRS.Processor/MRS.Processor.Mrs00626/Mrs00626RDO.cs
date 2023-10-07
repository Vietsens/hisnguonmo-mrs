using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00626
{
    public class Mrs00626RDO:V_HIS_BABY
    {
        public string TREATMENT_CODE { get; set; }	
        public string TDL_PATIENT_CODE { get; set; }	
        public string TDL_PATIENT_NAME { get; set; }	
        public long TDL_PATIENT_DOB { get; set; }	
        public string TDL_PATIENT_ADDRESS { get; set; }	

        public Mrs00626RDO()
        {
            // TODO: Complete member initialization
        }

        public Mrs00626RDO(HIS_TREATMENT r, V_HIS_BABY Baby)
        {
           
            try
            {
                if (r == null) r = new HIS_TREATMENT();
                this.TREATMENT_CODE = r.TREATMENT_CODE;
                this.TDL_PATIENT_CODE = r.TDL_PATIENT_CODE;
                this.TDL_PATIENT_NAME = r.TDL_PATIENT_NAME;
                this.TDL_PATIENT_DOB = r.TDL_PATIENT_DOB;
                this.TDL_PATIENT_ADDRESS = r.TDL_PATIENT_ADDRESS;
          
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_BABY>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(Baby)));
                }
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
