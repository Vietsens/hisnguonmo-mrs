using MRS.Processor.Mrs00585;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00585
{
    public class Mrs00585RDO : V_HIS_SERE_SERV
    {
        public decimal TOTAL_PRICE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long DOB { get; set; }
        public string GENDER_NAME { get; set; }
        public decimal TEIN_TOTAL_PRICE { get; set; }
        public decimal TEIN_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_TEIN_TOTAL_PRICE { get; set; }
        public decimal DIIM_TOTAL_PRICE { get; set; }
        public decimal DIIM_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_DIIM_TOTAL_PRICE { get; set; }
        public decimal SUIM_TOTAL_PRICE { get; set; }
        public decimal SUIM_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_SUIM_TOTAL_PRICE { get; set; }
        public decimal ECG_TOTAL_PRICE { get; set; }
        public decimal ECG_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_ECG_TOTAL_PRICE { get; set; }
        public decimal BRAIN_BLOOD_TOTAL_PRICE { get; set; }
        public decimal BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_BRAIN_BLOOD_TOTAL_PRICE { get; set; }
        public decimal OTHER_ENDO_TOTAL_PRICE { get; set; }
        public decimal OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_OTHER_ENDO_TOTAL_PRICE { get; set; }
        public decimal CERVICAL_ENDO_TOTAL_PRICE { get; set; }
        public decimal CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_CERVICAL_ENDO_TOTAL_PRICE { get; set; }
        public decimal PTTT_TOTAL_PRICE { get; set; }
        public decimal BED_TOTAL_PRICE { get; set; }
        public decimal EXAM_TOTAL_PRICE { get; set; }
        public decimal OTHER_FUEX_TOTAL_PRICE { get; set; }
        public decimal MEDICINE_TOTAL_PRICE { get; set; }
        public decimal EXPEND_MEDICINE_TOTAL_PRICE { get; set; }
        public decimal MATERIAL_TOTAL_PRICE { get; set; }
        public decimal EXPEND_MATERIAL_TOTAL_PRICE { get; set; }
        public decimal BLOOD_TOTAL_PRICE { get; set; }
        public decimal TRANS_TOTAL_PRICE { get; set; }
        public decimal DIFF_TOTAL_PRICE { get; set; }
        public long ROOM_NUM_ORDER { get; set; }

        public Mrs00585RDO(V_HIS_SERE_SERV r, Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat, Mrs00585Filter mrs00585Filter, List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter,List<HIS_EXECUTE_ROOM> listHistRoom,List<HIS_TRANSACTION> listHisTransaction)
        {
            PropertyInfo[] p = typeof(V_HIS_SERE_SERV).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtendField(dicServiceMety, dicServiceMaty, listServiceRetyCat, mrs00585Filter, lastHisPatientTypeAlter, listHistRoom, listHisTransaction);
        }

        public Mrs00585RDO()
        {

        }

        private void SetExtendField(Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat, Mrs00585Filter mrs00585Filter, List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter, List<HIS_EXECUTE_ROOM> listHistRoom, List<HIS_TRANSACTION> listHisTransaction)
        {
            #region Contructor object
            var room = listHistRoom.FirstOrDefault(o => o.ID == this.TDL_REQUEST_ROOM_ID) ?? new HIS_EXECUTE_ROOM();
            var patientTypeAlter = lastHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == this.TDL_TREATMENT_ID) ?? new HIS_PATIENT_TYPE_ALTER();
            var transaction = listHisTransaction.FirstOrDefault(o => o.TREATMENT_ID == this.TDL_TREATMENT_ID) ?? new HIS_TRANSACTION();
            var quotaExpend = dicServiceMety.ContainsKey(this.SERVICE_ID) ? dicServiceMety[this.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : (dicServiceMaty.ContainsKey(this.SERVICE_ID) ? dicServiceMaty[this.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : 0);
            var ecgServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__ECG).Select(q => q.SERVICE_ID).Distinct().ToList();
            var brainBloodServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__BRAIN_BLOOD).Select(q => q.SERVICE_ID).Distinct().ToList();
            var cervicalEndoServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__CERVICAL_ENDO).Select(q => q.SERVICE_ID).Distinct().ToList();
            var SHServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__SH).Select(q => q.SERVICE_ID).Distinct().ToList();
            var VSServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__VS).Select(q => q.SERVICE_ID).Distinct().ToList();
            var HHServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__HH).Select(q => q.SERVICE_ID).Distinct().ToList();
            var NTServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00585Filter.CATEGORY_CODE__NT).Select(q => q.SERVICE_ID).Distinct().ToList();
            
            #endregion

            #region Contructor field
            this.TREATMENT_CODE = this.TDL_TREATMENT_CODE;
            this.PATIENT_CODE = transaction.TDL_PATIENT_CODE;
            this.PATIENT_NAME = transaction.TDL_PATIENT_NAME;
            this.DOB = transaction.TDL_PATIENT_DOB ?? 0;
            this.GENDER_NAME = transaction.TDL_PATIENT_GENDER_NAME;
            this.ADDRESS = transaction.TDL_PATIENT_ADDRESS;
            var ToTalPatientPrice = this.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
            if (this.HEIN_RATIO.HasValue && this.HEIN_LIMIT_PRICE.HasValue && this.PRICE > this.HEIN_LIMIT_PRICE.Value)
            {
                ToTalPatientPrice = AMOUNT * (this.HEIN_LIMIT_PRICE.Value - this.HEIN_LIMIT_PRICE.Value*this.HEIN_RATIO.Value);
            }
            this.TOTAL_PRICE = ToTalPatientPrice;

            this.ROOM_NUM_ORDER = room.NUM_ORDER ?? 0;
            #endregion

            #region process with issue
            
            switch (this.TDL_SERVICE_TYPE_ID)
            {
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN:
                    this.TEIN_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_TEIN = this.AMOUNT;
                    this.TEIN_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_TEIN_TOTAL_PRICE = this.TEIN_TOTAL_PRICE - this.TEIN_QUOTA_EXPEND_TOTAL_PRICE;
                    if (SHServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.SH_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_SH=this.AMOUNT;
                        this.SH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_SH_TOTAL_PRICE = this.SH_TOTAL_PRICE - this.SH_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (VSServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.VS_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_VS = this.AMOUNT;
                        this.VS_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_VS_TOTAL_PRICE = this.VS_TOTAL_PRICE - this.VS_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (HHServiceIds.Contains(this.SERVICE_ID))
                     {
                         this.HH_TOTAL_PRICE = ToTalPatientPrice;
                         this._AM_HH = this.AMOUNT;
                         this.HH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                         this.DIFF_HH_TOTAL_PRICE = this.HH_TOTAL_PRICE - this.HH_QUOTA_EXPEND_TOTAL_PRICE;
                     }
                    else if (NTServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.NT_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_NT = this.AMOUNT;
                        this.NT_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_NT_TOTAL_PRICE = this.NT_TOTAL_PRICE - this.NT_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                   
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA:
                    this.DIIM_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_DIIM = this.AMOUNT;
                    this.DIIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_DIIM_TOTAL_PRICE = this.DIIM_TOTAL_PRICE - this.DIIM_QUOTA_EXPEND_TOTAL_PRICE;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA:
                    this.SUIM_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_SUIM = this.AMOUNT;
                    this.SUIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_SUIM_TOTAL_PRICE = this.SUIM_TOTAL_PRICE - this.SUIM_QUOTA_EXPEND_TOTAL_PRICE;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN:
                    if (ecgServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.ECG_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_ECG = this.AMOUNT;
                        this.ECG_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_ECG_TOTAL_PRICE = this.ECG_TOTAL_PRICE - this.ECG_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (brainBloodServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.BRAIN_BLOOD_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_BRAIN_BLOOD = this.AMOUNT;
                        this.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_BRAIN_BLOOD_TOTAL_PRICE = this.BRAIN_BLOOD_TOTAL_PRICE - this.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else
                    {
                        this.OTHER_FUEX_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_OTHER_FUEX = this.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS:
                    if (cervicalEndoServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.CERVICAL_ENDO_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_CERVICAL_ENDO = this.AMOUNT;
                        this.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_CERVICAL_ENDO_TOTAL_PRICE = this.CERVICAL_ENDO_TOTAL_PRICE - this.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else
                    {
                        this.OTHER_ENDO_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_OTHER_ENDO = this.AMOUNT;
                        this.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_OTHER_ENDO_TOTAL_PRICE = this.OTHER_ENDO_TOTAL_PRICE - this.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT:
                    this.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_PTTT = this.AMOUNT;
                    this.PT_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_PT = this.AMOUNT;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT:
                    this.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_PTTT = this.AMOUNT;
                    this.TT_TOTAL_PRICE = ToTalPatientPrice;
                     this._AM_TT= this.AMOUNT;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G:
                    this.BED_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_BED = this.AMOUNT;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH:
                    {
                        this.EXAM_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_EXAM = this.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC:
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.EXPEND_MEDICINE_TOTAL_PRICE = this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        this._AM_EXPEND_MEDICINE = this.AMOUNT;
                    }
                    else
                    {
                        this.MEDICINE_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_MEDICINE = this.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT:
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.EXPEND_MATERIAL_TOTAL_PRICE = this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        this._AM_EXPEND_MATERIAL = this.AMOUNT;
                    }
                    else
                    {
                        this.MATERIAL_TOTAL_PRICE = ToTalPatientPrice;
                        this._AM_MATERIAL = this.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU:
                    this.BLOOD_TOTAL_PRICE = ToTalPatientPrice;
                    this._AM_BLOOD = this.AMOUNT;
                    break;
            }
            if (mrs00585Filter.SERVICE_CODE__TRANs != null && mrs00585Filter.SERVICE_CODE__TRANs.Contains(this.TDL_SERVICE_CODE))
            {
                this.TRANS_TOTAL_PRICE = ToTalPatientPrice;
                this._AM_TRANS = this.AMOUNT;
            }
            this.DIFF_TOTAL_PRICE = this.TOTAL_PRICE - quotaExpend - this.MEDICINE_TOTAL_PRICE - this.EXPEND_MEDICINE_TOTAL_PRICE - this.MATERIAL_TOTAL_PRICE - this.EXPEND_MATERIAL_TOTAL_PRICE;
            #endregion
        }

        public decimal SH_TOTAL_PRICE { get; set; }

        public decimal VS_TOTAL_PRICE { get; set; }

        public decimal HH_TOTAL_PRICE { get; set; }

        public decimal NT_TOTAL_PRICE { get; set; }

        public decimal VS_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_VS_TOTAL_PRICE { get; set; }

        public decimal HH_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_HH_TOTAL_PRICE { get; set; }

        public decimal NT_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_NT_TOTAL_PRICE { get; set; }

        public decimal SH_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_SH_TOTAL_PRICE { get; set; }

        public decimal _AM_TEIN { get; set; }

        public decimal _AM_SH { get; set; }

        public decimal _AM_VS { get; set; }

        public decimal _AM_HH { get; set; }

        public decimal _AM_NT { get; set; }

        public decimal _AM_DIIM { get; set; }

        public decimal _AM_SUIM { get; set; }

        public decimal _AM_ECG { get; set; }

        public decimal _AM_BRAIN_BLOOD { get; set; }

        public decimal _AM_OTHER_FUEX { get; set; }

        public decimal _AM_CERVICAL_ENDO { get; set; }

        public decimal _AM_OTHER_ENDO { get; set; }

        public decimal _AM_PTTT { get; set; }

        public decimal _AM_BED { get; set; }

        public decimal _AM_EXAM { get; set; }

        public decimal _AM_EXPEND_MEDICINE { get; set; }

        public decimal _AM_MEDICINE { get; set; }

        public decimal _AM_EXPEND_MATERIAL { get; set; }

        public decimal _AM_MATERIAL { get; set; }

        public decimal _AM_BLOOD { get; set; }

        public decimal PT_TOTAL_PRICE { get; set; }

        public decimal _AM_PT { get; set; }

        public decimal TT_TOTAL_PRICE { get; set; }

        public decimal _AM_TT { get; set; }

        public decimal _AM_TRANS { get; set; }

        public string ADDRESS { get; set; }

        public decimal TOTAL_PRICE_FEE { get; set; }
    }
}
