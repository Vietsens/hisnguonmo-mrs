using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00525
{
    class Mrs00525RDO : V_HIS_TRANSACTION
    {
        public string CREATE_DATE_STR { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string IS_OUT { get; set; }
        public decimal? ELE_AMOUNT { get; set; }

        public Mrs00525RDO(V_HIS_TRANSACTION Deposit, Dictionary<long, HIS_TREATMENT> dicTreatment, List<string> listEle)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TRANSACTION>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(Deposit)));
                }
                SetExtendField(this, dicTreatment,listEle);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00525RDO rdo, Dictionary<long, HIS_TREATMENT> dicTreatment,List<string> listEle)
        {
            try
            {
                TRANSACTION_CODE = rdo.TRANSACTION_CODE;
                PATIENT_CODE = rdo.TDL_PATIENT_CODE;
                VIR_PATIENT_NAME = rdo.TDL_PATIENT_NAME;
                DESCRIPTION = rdo.DESCRIPTION;
                AMOUNT = rdo.AMOUNT;
                if (listEle != null && string.IsNullOrEmpty(rdo.PAY_FORM_CODE)==false && listEle.Contains(rdo.PAY_FORM_CODE))
                {
                    ELE_AMOUNT = rdo.AMOUNT;
                }
                CASHIER_USERNAME = rdo.CASHIER_USERNAME;
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CREATE_TIME ?? 0);
                var treatment = dicTreatment.ContainsKey(TREATMENT_ID ?? 0) ? dicTreatment[TREATMENT_ID ?? 0] : null;
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
