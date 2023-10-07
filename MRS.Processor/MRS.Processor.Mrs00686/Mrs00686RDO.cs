using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00686
{
    public class Mrs00686RDO : V_HIS_TRANSACTION
    {
        public string TRANSACTION_TIME_STR { get; set; }
        public Decimal DEP_BIL { get; set; }
        public Decimal REPAYs { get; set; }
        public Decimal KC_AMOUNTs { get; set; }
        public string NUM_ORDER_STR { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public Decimal REDIASUAL_AMOUNT { get; set; }
        public Decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Decimal TOTAL_PATIENT_PRICE_VP { get; set; }
        public Mrs00686RDO()
        {

        }

        public Mrs00686RDO(V_HIS_TRANSACTION data, List<V_HIS_BILL_FUND> ListBillFund, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, List<V_HIS_TREATMENT_FEE> listTreatmentFeeView, List<TYPE_PRICE> listTypePrice)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TRANSACTION>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }

                this.VIR_PATIENT_NAME = TDL_PATIENT_NAME;
                SetExtendField(this, ListBillFund, listPatientTypeAlter, listTreatmentFeeView, listTypePrice);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00686RDO rdo, List<V_HIS_BILL_FUND> ListBillFund, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, List<V_HIS_TREATMENT_FEE> listTreatmentFeeView, List<TYPE_PRICE> listTypePrice)
        {
            try
            {
                Decimal totalFund = 0;
                if (this.TREATMENT_TYPE_ID == null || this.TREATMENT_TYPE_ID == 0)
                {
                    this.TREATMENT_TYPE_ID = (listPatientTypeAlter.OrderBy(p => p.ID).ThenBy(q => q.LOG_TIME).LastOrDefault(o => o.TREATMENT_ID == this.TREATMENT_ID && o.LOG_TIME <= this.TRANSACTION_TIME) ?? listPatientTypeAlter.OrderBy(p => p.ID).ThenBy(q => q.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == this.TREATMENT_ID && o.LOG_TIME > this.TRANSACTION_TIME) ?? new HIS_PATIENT_TYPE_ALTER()).TREATMENT_TYPE_ID;
                }
                List<V_HIS_BILL_FUND> ListBillFundSub = new List<V_HIS_BILL_FUND>();
                ListBillFundSub = ListBillFund.Where(o => o.ID == rdo.ID).ToList();
                if (ListBillFundSub.Count > 0 && rdo.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) totalFund = ListBillFundSub.Sum(o => o.AMOUNT);
                TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.TRANSACTION_TIME);
                if (this.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                    REPAYs = AMOUNT;
                if (this.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                    DEP_BIL += AMOUNT;
                if (this.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    DEP_BIL = rdo.AMOUNT - (rdo.EXEMPTION ?? 0) - totalFund;
                    KC_AMOUNTs = rdo.KC_AMOUNT ?? 0;
                }
                this.TRANSACTION_TYPE_NAME = (rdo.TDL_SERE_SERV_DEPOSIT_COUNT > 0) ? "Tạm ứng dịch vụ" :
                     ((rdo.TDL_SESE_DEPO_REPAY_COUNT > 0) ? "Hoàn ứng dịch vụ" : this.TRANSACTION_TYPE_NAME);
                this.NUM_ORDER_STR = string.Format("{0:0000000}", Convert.ToInt64(NUM_ORDER));
                if (listTypePrice != null && listTypePrice.Count > 0)
                {
                    var typePrice = listTypePrice.FirstOrDefault(o => o.BILL_ID == this.ID);
                    if (typePrice != null)
                    {
                        this.TOTAL_PATIENT_PRICE_BHYT = typePrice.TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        this.TOTAL_PATIENT_PRICE_VP = typePrice.TOTAL_PATIENT_PRICE_VP ?? 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
    public class TYPE_PRICE
    {
        public long BILL_ID { get; set; }
        public Decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Decimal? TOTAL_PATIENT_PRICE_VP { get; set; }
    }
}
