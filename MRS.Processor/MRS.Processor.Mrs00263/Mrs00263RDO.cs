using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00263
{
    public class Mrs00263RDO : HIS_TRANSACTION
    {
        public string CREATE_DATE_STR { get; set; }
        //public string TRANSACTION_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        //public string DESCRIPTION { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string IS_OUT { get;  set;  }

        //public decimal AMOUNT { get;  set;  }

        public Mrs00263RDO(HIS_TRANSACTION Deposit)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_TRANSACTION>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(Deposit)));   
                }
                SetExtendField(this); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00263RDO rdo)
        {

            try
            {
                PATIENT_CODE = rdo.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = rdo.TDL_PATIENT_NAME; 
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CREATE_TIME ?? 0); 
                ProcessIsOutTreatment(rdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessIsOutTreatment(HIS_TRANSACTION deposit)
        {
            try
            {
                var treatment = new HisTreatmentManager().GetViewById(deposit.TREATMENT_ID ?? 0); 
                if (treatment != null)
                {
                    if (treatment.TREATMENT_END_TYPE_ID > 0)
                    {
                        IS_OUT = "Đã ra"; 
                    }
                    else
                    {
                        IS_OUT = "Chưa ra"; 
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
