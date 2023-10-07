using MRS.Processor.Mrs00562;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Repository;

namespace MRS.Proccessor.Mrs00562
{
    public class Mrs00562RDO
    {
       public string HEIN_SERVICE_BHYT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_STT { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public long SERVICE_UNIT_ID { get; set; }
        
        public string SERVICE_NAME { get; set; }
        
        public decimal AMOUNT_NGOAITRU_SUM { get; set; }
        public decimal AMOUNT_NOITRU_SUM { get; set; }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE_SUM { get; set; }

        public long? SERVICE_ID { get; set; }

        public long HEIN_SERVICE_TYPE_NUM_ORDER { get; set; }
        public long HEIN_SERVICE_TYPE_ID { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }

        public long HEIN_CARD_NUMBER_TYPE_ID { get; set; }


        private HIS_HEIN_APPROVAL heinApproval = null;
        public HIS_SERE_SERV sereServ = null;
        private HIS_TREATMENT_TYPE treatmentType = null;
        private Mrs00562Filter filter = null;
        private string NumDigits;

        public Mrs00562RDO(string NumDigits, HIS_SERE_SERV r, List<HIS_HEIN_APPROVAL> listHisHeinApproval, Mrs00562Filter filter)
        {
            this.NumDigits = NumDigits;
            this.heinApproval = listHisHeinApproval.FirstOrDefault(o => o.ID == r.HEIN_APPROVAL_ID) ?? new HIS_HEIN_APPROVAL();
            this.treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
            this.filter = filter;
            this.sereServ = r;
            
            this.ChargeKey();
        }

        private void ChargeKey()
        {
            this.ServiceInfo();
            this.PriceInfo();
            this.ServiceTypeInfo();
            this.HeinCardTypeInfo();
        }

        private void ServiceInfo()
        {
            this.HEIN_SERVICE_BHYT_NAME = this.sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
            this.HEIN_SERVICE_BHYT_STT = this.sereServ.TDL_HEIN_ORDER;
            this.SERVICE_NAME = this.sereServ.TDL_SERVICE_NAME;
            this.HEIN_SERVICE_BHYT_CODE = this.sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
            this.SERVICE_UNIT_ID = this.sereServ.TDL_SERVICE_UNIT_ID;
        }

       
        private void PriceInfo()
        {
           
            if (this.treatmentType.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
            {
                this.AMOUNT_NGOAITRU_SUM = this.sereServ.AMOUNT;
            }
            if (this.treatmentType.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
            {
                this.AMOUNT_NOITRU_SUM = this.sereServ.AMOUNT;
            }

            this.PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);

            this.TOTAL_PRICE_SUM = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits, sereServ, new HIS_BRANCH());
        }

        private void ServiceTypeInfo()
        {
            var heinServiceType = HisHeinServiceTypeCFG.HEIN_SERVICE_TYPEs.FirstOrDefault(o => o.ID == (this.sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0)) ?? new HIS_HEIN_SERVICE_TYPE();
            this.SERVICE_ID = this.sereServ.SERVICE_ID;
            this.HEIN_SERVICE_TYPE_ID = this.sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0;
            this.HEIN_SERVICE_TYPE_NAME = heinServiceType.HEIN_SERVICE_TYPE_NAME??"Chưa có loại dịch vụ";
            this.HEIN_SERVICE_TYPE_NUM_ORDER = heinServiceType.NUM_ORDER ?? 1000;

            
        }

        private void HeinCardTypeInfo()
        {
            if (this.sereServ.HEIN_CARD_NUMBER != null && this.sereServ.HEIN_CARD_NUMBER != "")
            {
                if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01 != null
                    && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01.Exists(o => this.sereServ.HEIN_CARD_NUMBER.StartsWith(o)))
                {
                    this.HEIN_CARD_NUMBER_TYPE_ID = 1;
                }
                else if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02 != null
                    && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02.Exists(o => this.sereServ.HEIN_CARD_NUMBER.StartsWith(o)))
                {
                    this.HEIN_CARD_NUMBER_TYPE_ID = 2;
                }
                else
                    this.HEIN_CARD_NUMBER_TYPE_ID = 3;
            }
        }
        
        public Mrs00562RDO()
        {

        }
    }
}
