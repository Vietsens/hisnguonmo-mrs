using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00071
{
    class Mrs00071RDO : HIS_TRANSACTION
    {
        public string TRANSACTION_TIME_STR { get; set; }

        public string TREATMENT_TYPE_NAME { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal DEPOSIT_AMOUNT { get; set; }

        public decimal REPAY_AMOUNT { get; set; }

        public decimal CANCEL_AMOUNT { get; set; }

        public decimal REDIASUAL_AMOUNT { get; set; }


        public Mrs00071RDO() { }

        public Mrs00071RDO(HIS_TRANSACTION data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_TRANSACTION>(); 
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

        private void SetExtendField(Mrs00071RDO rdo)
        {
            try
            {
                this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.TRANSACTION_TIME);
                if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT || (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && rdo.TDL_SERE_SERV_DEPOSIT_COUNT > 0))
                {
                    this.BILL_AMOUNT = rdo.AMOUNT;
                    if (rdo.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.CANCEL_AMOUNT = rdo.AMOUNT;
                    }
                }
                else if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                {
                    this.DEPOSIT_AMOUNT = rdo.AMOUNT;

                    if (rdo.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.CANCEL_AMOUNT = rdo.AMOUNT;
                    }
                }
                else if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    this.REPAY_AMOUNT = rdo.AMOUNT;

                    if (rdo.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.CANCEL_AMOUNT = -rdo.AMOUNT;
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
