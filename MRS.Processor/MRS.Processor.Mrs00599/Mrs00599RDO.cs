using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00599
{
    class Mrs00599RDO :V_HIS_TRANSACTION
    {
        public string TRANSACTION_TIME_STR { get; set; }
        public string TIG_TRANSACTION_TIME_STR { get; set; }	
        public decimal PRICE__SALE { get; set; }
        public decimal PRICE__VOID_REVERT { get; set; }
        public decimal PRICE__TRANSFER_IN { get; set; }
        public decimal PRICE__TRANSFER_OUT { get; set; }

        public Mrs00599RDO()
        {

        }

        public Mrs00599RDO(V_HIS_TRANSACTION data)
        {
            try
            {
                 System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TRANSACTION>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));   
                }
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00599RDO r)
        {
            try
            {
                this.TIG_TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.TIG_TRANSACTION_TIME??0);
                this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.TRANSACTION_TIME);
                if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    this.PRICE__SALE = r.AMOUNT;
                }
                else if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                {
                    this.PRICE__TRANSFER_IN = r.AMOUNT;
                }
                else if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    this.PRICE__TRANSFER_OUT = r.AMOUNT;
                }
                if (r.TIG_VOID_CODE!=null)
                {
                    this.PRICE__VOID_REVERT = r.AMOUNT;
                }
                
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

       
    }
}
