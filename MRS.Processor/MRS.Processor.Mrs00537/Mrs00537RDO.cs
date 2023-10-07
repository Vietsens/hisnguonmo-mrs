using MRS.Processor.Mrs00537;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Repository;

namespace MRS.Proccessor.Mrs00537
{
    public class Mrs00537RDO
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

        public string IS_RIGHT_ROUTE { get; set; }
        public string ROUTE_CODE { get; set; }

        private HIS_HEIN_APPROVAL heinApproval = null;
        public HIS_SERE_SERV sereServ = null;
        private HIS_TREATMENT_TYPE treatmentType = null;
        private List<HIS_BRANCH> branchs = null;
        private Mrs00537Filter filter = null;

        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";
        private string NumDigits;

        public Mrs00537RDO(string NumDigits, HIS_SERE_SERV r, List<HIS_HEIN_APPROVAL> listHisHeinApproval, Mrs00537Filter filter)
        {
            this.NumDigits = NumDigits;
            this.heinApproval = listHisHeinApproval.FirstOrDefault(o => o.ID == r.HEIN_APPROVAL_ID) ?? new HIS_HEIN_APPROVAL();
            this.treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
            this.filter = filter;
            this.sereServ = r;
            this.branchs = MRS.MANAGER.Config.HisBranchCFG.HisBranchs;
            if (this.filter.BRANCH_ID != null)
            {
                this.branchs = this.branchs.Where(o => o.ID == this.filter.BRANCH_ID).ToList();
            }
            
            this.ChargeKey();
        }

        private void ChargeKey()
        {
            this.ServiceInfo();
            this.PriceInfo();
            this.ServiceTypeInfo();
            this.HeinCardTypeInfo();
            this.RouteInfo();
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

            this.TOTAL_PRICE_SUM =Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, new HIS_BRANCH());
        }

        private void ServiceTypeInfo()
        {
            if (filter.IS_BY_SERVICE_TYPE == true)
            {
                var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == this.sereServ.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                this.SERVICE_ID = this.sereServ.SERVICE_ID;
                this.HEIN_SERVICE_TYPE_ID = this.sereServ.TDL_SERVICE_TYPE_ID;
                this.HEIN_SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME ?? "Chưa có loại dịch vụ";
                this.HEIN_SERVICE_TYPE_NUM_ORDER = serviceType.NUM_ORDER ?? 1000;
            }
            else
            {
                var heinServiceType = HisHeinServiceTypeCFG.HEIN_SERVICE_TYPEs.FirstOrDefault(o => o.ID == (this.sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0)) ?? new HIS_HEIN_SERVICE_TYPE();
                this.SERVICE_ID = this.sereServ.SERVICE_ID;
                this.HEIN_SERVICE_TYPE_ID = this.sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0;
                this.HEIN_SERVICE_TYPE_NAME = heinServiceType.HEIN_SERVICE_TYPE_NAME ?? "Chưa có loại dịch vụ";
                this.HEIN_SERVICE_TYPE_NUM_ORDER = heinServiceType.NUM_ORDER ?? 1000;
            }
            
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
        private void RouteInfo()
        {
            if (this.filter.IS_ROUTE ?? false)
            {
                if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__All.Exists(o => this.sereServ.HEIN_CARD_NUMBER.StartsWith(o)))
                {
                    if (this.branchs.Exists(o => o.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(this.heinApproval.HEIN_MEDI_ORG_CODE)))
                    {
                        this.ROUTE_CODE = RouteCodeA;
                    }
                    else if (this.heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {
                        this.ROUTE_CODE = RouteCodeB;
                        this.IS_RIGHT_ROUTE = MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE;
                    }
                    else
                    {
                        this.ROUTE_CODE = RouteCodeB;
                        this.IS_RIGHT_ROUTE = MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE;
                    }
                }
                else
                {
                    if (this.branchs.Exists(o => o.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(this.heinApproval.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(this.heinApproval.HEIN_CARD_NUMBER)))
                    {
                        this.ROUTE_CODE = RouteCodeA;
                    }
                    else if (checkBhytProvinceCode(this.heinApproval.HEIN_CARD_NUMBER))
                    {
                        this.ROUTE_CODE = RouteCodeB;
                    }
                    else
                    {
                        this.ROUTE_CODE = RouteCodeC;
                    }
                }
            }
        }
        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
                {
                    string provinceCode = HeinNumber.Substring(3, 2);
                    if (this.branchs.Exists(o => o.HEIN_PROVINCE_CODE.Equals(provinceCode)))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        public Mrs00537RDO()
        {

        }
    }
}
