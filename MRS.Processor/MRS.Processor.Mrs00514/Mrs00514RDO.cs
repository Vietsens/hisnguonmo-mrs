using MRS.Processor.Mrs00514;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00514
{
    public class Mrs00514RDO : V_HIS_SERE_SERV
    {
        public string FEE_LOCK_TIME { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long TREATMENT_BHYT_END_ROOM_ID { get; set; }//
        public decimal TEIN_TOTAL_PRICE { get; set; }
        public decimal COUNT_TREATMENT_EXECUTE_ROOM { get; set; }//
        public decimal COUNT_TREATMENT_BHYT_END_ROOM { get; set; }//
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

        public Mrs00514RDO(V_HIS_SERE_SERV r, Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat, Mrs00514Filter mrs00514Filter, List<V_HIS_TREATMENT_4> treatments, List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter,List<HIS_EXECUTE_ROOM> listHistRoom)
        {
            PropertyInfo[] p = typeof(V_HIS_SERE_SERV).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtendField(dicServiceMety, dicServiceMaty, listServiceRetyCat, mrs00514Filter, treatments, lastHisPatientTypeAlter, listHistRoom);
        }

        public Mrs00514RDO()
        {

        }

        private void SetExtendField(Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat, Mrs00514Filter mrs00514Filter, List<V_HIS_TREATMENT_4> treatments, List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter, List<HIS_EXECUTE_ROOM> listHistRoom)
        {
            #region Contructor object
            var treatment = treatments.FirstOrDefault(o => o.ID == this.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT_4();
            var room = listHistRoom.FirstOrDefault(o => o.ID == this.TDL_REQUEST_ROOM_ID) ?? new HIS_EXECUTE_ROOM();
            var patientTypeAlter = lastHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == this.TDL_TREATMENT_ID) ?? new HIS_PATIENT_TYPE_ALTER();
            var quotaExpend = dicServiceMety.ContainsKey(this.SERVICE_ID) ? dicServiceMety[this.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : (dicServiceMaty.ContainsKey(this.SERVICE_ID) ? dicServiceMaty[this.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : 0);
            var ecgServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00514Filter.CATEGORY_CODE__ECG).Select(q => q.SERVICE_ID).Distinct().ToList();
            var brainBloodServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00514Filter.CATEGORY_CODE__BRAIN_BLOOD).Select(q => q.SERVICE_ID).Distinct().ToList();
            var cervicalEndoServiceIds = listServiceRetyCat.Where(o => o.CATEGORY_CODE == mrs00514Filter.CATEGORY_CODE__CERVICAL_ENDO).Select(q => q.SERVICE_ID).Distinct().ToList();
            #endregion

            #region Contructor field
            this.TREATMENT_CODE = this.TDL_TREATMENT_CODE;
            this.FEE_LOCK_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
            this.TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
            this.ROOM_NUM_ORDER = room.NUM_ORDER ?? 0;
            #endregion

            #region process with issue
            if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.TREATMENT_BHYT_END_ROOM_ID = treatment.END_ROOM_ID ?? 0;
            }
            switch (this.TDL_SERVICE_TYPE_ID)
            {
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN:
                    this.TEIN_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    this.TEIN_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_TEIN_TOTAL_PRICE = this.TEIN_TOTAL_PRICE - this.TEIN_QUOTA_EXPEND_TOTAL_PRICE;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA:
                    this.DIIM_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    this.DIIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_DIIM_TOTAL_PRICE = this.DIIM_TOTAL_PRICE - this.DIIM_QUOTA_EXPEND_TOTAL_PRICE;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA:
                    this.SUIM_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    this.SUIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    this.DIFF_SUIM_TOTAL_PRICE = this.SUIM_TOTAL_PRICE - this.SUIM_QUOTA_EXPEND_TOTAL_PRICE;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN:
                    if (ecgServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.ECG_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                        this.ECG_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_ECG_TOTAL_PRICE = this.ECG_TOTAL_PRICE - this.ECG_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (brainBloodServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.BRAIN_BLOOD_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                        this.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_BRAIN_BLOOD_TOTAL_PRICE = this.BRAIN_BLOOD_TOTAL_PRICE - this.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else
                    {
                        this.OTHER_FUEX_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS:
                    if (cervicalEndoServiceIds.Contains(this.SERVICE_ID))
                    {
                        this.CERVICAL_ENDO_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                        this.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_CERVICAL_ENDO_TOTAL_PRICE = this.CERVICAL_ENDO_TOTAL_PRICE - this.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else
                    {
                        this.OTHER_ENDO_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                        this.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        this.DIFF_OTHER_ENDO_TOTAL_PRICE = this.OTHER_ENDO_TOTAL_PRICE - this.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT:
                    this.PTTT_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT:
                    this.PTTT_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G:
                    this.BED_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH:
                    this.EXAM_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC:
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.EXPEND_MEDICINE_TOTAL_PRICE = this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                    }
                    else
                    {
                        this.MEDICINE_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT:
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.EXPEND_MATERIAL_TOTAL_PRICE = this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                    }
                    else
                    {
                        this.MATERIAL_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU:
                    this.BLOOD_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;
                    break;
            }
            if (this.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                this.TRANS_TOTAL_PRICE = this.VIR_TOTAL_PRICE ?? 0;

            this.DIFF_TOTAL_PRICE = this.TOTAL_PRICE - quotaExpend - this.MEDICINE_TOTAL_PRICE - this.EXPEND_MEDICINE_TOTAL_PRICE - this.MATERIAL_TOTAL_PRICE - this.EXPEND_MATERIAL_TOTAL_PRICE;
            #endregion
        }
    }
}
