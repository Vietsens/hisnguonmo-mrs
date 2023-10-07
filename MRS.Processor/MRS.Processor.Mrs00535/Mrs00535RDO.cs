using MRS.Processor.Mrs00535;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Repository;

namespace MRS.Proccessor.Mrs00535
{
    public class Mrs00535RDO
    {
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_HOATCHAT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_STT { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }

        public string IS_RIGHT_ROUTE { get; set; }
        public string ROUTE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public decimal IMP_PRICE { get; set; } 

        public decimal AMOUNT_NGOAITRU_SUM { get; set; }
        public decimal AMOUNT_NOITRU_SUM { get; set; }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE_SUM { get; set; }

        public long? SERVICE_ID { get; set; }

        public long HEIN_SERVICE_TYPE_ID { get; set; }

        public long HEIN_CARD_NUMBER_TYPE_ID { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }

        public string MEDICINE_LINE_NAME { get; set; }

        private HIS_HEIN_APPROVAL heinApproval = null;
        public HIS_SERE_SERV sereServ = null;
        private V_HIS_MEDICINE_TYPE medicineType = null;
        private HIS_MEDICINE medicine = null;
        private HIS_TREATMENT_TYPE treatmentType = null;
        private Mrs00535Filter filter = null;
        private List<HIS_BRANCH> branchs = null;
        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";
        private string NumDigits;

        public Mrs00535RDO(string NumDigits, HIS_SERE_SERV r, List<HIS_HEIN_APPROVAL> listHisHeinApproval, List<V_HIS_MEDICINE_TYPE> listHisMedicineType,  List<long> HeinServiceTypeId_Medi, List<HIS_MEDICINE> listHisMedicine, Mrs00535Filter filter)
        {
            this.NumDigits = NumDigits;
            this.heinApproval = listHisHeinApproval.FirstOrDefault(o => o.ID == r.HEIN_APPROVAL_ID) ?? new HIS_HEIN_APPROVAL();
            this.treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
            this.filter = filter;
            this.sereServ = r;
            if (HeinServiceTypeId_Medi.Contains(r.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
            {
                this.medicineType = listHisMedicineType.FirstOrDefault(o => o.SERVICE_ID == r.SERVICE_ID) ?? new V_HIS_MEDICINE_TYPE();
                this.medicine = listHisMedicine.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            }

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
            this.MedicineInfo();
            this.PriceInfo();
            this.LineInfo();
            this.HeinCardTypeInfo();
            this.RouteInfo();
        }

        private void ServiceInfo()
        {
            this.HEIN_SERVICE_BHYT_NAME = this.sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
            this.HEIN_SERVICE_BHYT_STT = this.sereServ.TDL_HEIN_ORDER;
            this.SERVICE_NAME = this.sereServ.TDL_SERVICE_NAME;
            this.HEIN_SERVICE_BHYT_CODE = this.sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
        }

        private void MedicineInfo()
        {
            if (medicineType != null)
            {
                this.MEDICINE_HOATCHAT_NAME = this.medicineType.ACTIVE_INGR_BHYT_NAME;
                this.MEDICINE_HOATCHAT_CODE = this.medicineType.ACTIVE_INGR_BHYT_CODE;
                this.MEDICINE_DUONGDUNG_NAME = this.medicineType.MEDICINE_USE_FORM_NAME;
                this.MEDICINE_HAMLUONG_NAME = this.medicineType.CONCENTRA;
                this.MEDICINE_SODANGKY_NAME = this.medicineType.REGISTER_NUMBER;
                this.SERVICE_UNIT_NAME = this.medicineType.SERVICE_UNIT_NAME;
                this.BID_NUM_ORDER = this.medicine.TDL_BID_NUM_ORDER;
                this.BID_NUMBER = this.medicine.TDL_BID_NUMBER;
                this.MEDICINE_LINE_ID = this.medicineType.MEDICINE_LINE_ID;
                this.MEDICINE_LINE_NAME = this.medicineType.MEDICINE_LINE_NAME;

                this.PACKING_TYPE_NAME = this.medicineType.PACKING_TYPE_NAME;
                this.IMP_PRICE = this.medicine.IMP_PRICE * (this.medicine.IMP_VAT_RATIO + 1);
            }
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

            this.TOTAL_PRICE_SUM = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, new HIS_BRANCH());
        }

        private void LineInfo()
        {
            this.SERVICE_ID = this.sereServ.SERVICE_ID;
            this.HEIN_SERVICE_TYPE_ID = this.sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0;
            if (this.sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
            {
                if (this.filter.BLOOD_IS_GENERIC_MEDI)
                {
                    this.MEDICINE_LINE_ID = IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD;
                    this.MEDICINE_LINE_NAME = HisMedicineLineCFG.MEDICINE_LINEs.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD).MEDICINE_LINE_NAME;
                }
                else
                {
                    this.MEDICINE_LINE_ID = 1000;
                    this.MEDICINE_LINE_NAME = "Loại Khác";
                }
            }
            if (this.sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
            {
                this.MEDICINE_LINE_ID = 1000;
                this.MEDICINE_LINE_NAME = "Loại Khác";
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
                    if (this.branchs.Exists(o=>o.HEIN_PROVINCE_CODE.Equals(provinceCode)))
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
        public Mrs00535RDO()
        {

        }
    }
}
