using MRS.Processor.Mrs00561;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Repository;

namespace MRS.Proccessor.Mrs00561
{
    public class Mrs00561RDO
    {
        public string MATERIAL_HOATCHAT_NAME { get; set; }
        public string MATERIAL_HOATCHAT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_STT { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string MATERIAL_DUONGDUNG_NAME { get; set; }
        public string MATERIAL_HAMLUONG_NAME { get; set; }
        public string MATERIAL_SODANGKY_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }

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

        private HIS_HEIN_APPROVAL heinApproval = null;
        public HIS_SERE_SERV sereServ = null;
        private V_HIS_MATERIAL_TYPE materialType = null;
        private HIS_MATERIAL material = null;
        private HIS_TREATMENT_TYPE treatmentType = null;
        private Mrs00561Filter filter = null;
        private string NumDigits;

        public Mrs00561RDO(string NumDigits, HIS_SERE_SERV r, List<HIS_HEIN_APPROVAL> listHisHeinApproval, List<V_HIS_MATERIAL_TYPE> listHisMaterialType,  List<long> HeinServiceTypeId_Mate, List<HIS_MATERIAL> listHisMaterial, Mrs00561Filter filter)
        {
            this.NumDigits = NumDigits;
            this.heinApproval = listHisHeinApproval.FirstOrDefault(o => o.ID == r.HEIN_APPROVAL_ID) ?? new HIS_HEIN_APPROVAL();
            this.treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
            this.filter = filter;
            this.sereServ = r;
            if (HeinServiceTypeId_Mate.Contains(r.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
            {
                this.materialType = listHisMaterialType.FirstOrDefault(o => o.SERVICE_ID == r.SERVICE_ID) ?? new V_HIS_MATERIAL_TYPE();
                this.material = listHisMaterial.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            }

            
            this.ChargeKey();
        }

        private void ChargeKey()
        {
            this.ServiceInfo();
            this.MaterialInfo();
            this.PriceInfo();
            this.HeinCardTypeInfo();
        }

        private void ServiceInfo()
        {
            this.HEIN_SERVICE_BHYT_NAME = this.sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
            this.HEIN_SERVICE_BHYT_STT = this.sereServ.TDL_HEIN_ORDER;
            this.SERVICE_NAME = this.sereServ.TDL_SERVICE_NAME;
            this.HEIN_SERVICE_BHYT_CODE = this.sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
        }

        private void MaterialInfo()
        {
            if (materialType != null)
            {
               this.MATERIAL_HOATCHAT_NAME = null;//this.materialType.ACTIVE_INGR_BHYT_NAME;
               this.MATERIAL_DUONGDUNG_NAME = null;//this.materialType.MATERIAL_USE_FORM_NAME;
               this.MATERIAL_HAMLUONG_NAME = this.materialType.CONCENTRA;
               this.MATERIAL_SODANGKY_NAME = null;//this.materialType.REGISTER_NUMBER;
                this.SERVICE_UNIT_NAME = this.materialType.SERVICE_UNIT_NAME;
                this.BID_NUM_ORDER = this.material.TDL_BID_NUM_ORDER;
                this.BID_NUMBER = this.material.TDL_BID_NUMBER;

                this.PACKING_TYPE_NAME = this.materialType.PACKING_TYPE_NAME;
                this.IMP_PRICE = this.material.IMP_PRICE * (this.material.IMP_VAT_RATIO+ 1);
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

            this.TOTAL_PRICE_SUM = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits, sereServ,  new HIS_BRANCH());
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

        public Mrs00561RDO()
        {

        }
    }
}
