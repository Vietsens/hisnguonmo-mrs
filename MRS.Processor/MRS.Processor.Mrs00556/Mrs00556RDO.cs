using MRS.Processor.Mrs00556;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00556
{
    public class Mrs00556RDO : V_HIS_SERE_SERV
    {
        public long NUM_ORDER { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal VIR_TOTAL_PATIENT_PRICE_FEE { get; set; }
        public decimal VIR_TOTAL_PATIENT_PRICE_XHH { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long DOB { get; set; }
        public string GENDER_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public decimal TEIN_TOTAL_PRICE { get; set; }
        public decimal COUNT_TREATMENT_EXECUTE_ROOM { get; set; }//
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

        public decimal NSTH_TOTAL_PRICE { get; set; }
        public decimal NSTH_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_NSTH_TOTAL_PRICE { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string TDL_EXECUTE_USERNAME { get; set; }
        public string TDL_EXECUTE_LOGINNAME { get; set; }
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
        public decimal Z_TOTAL_PRICE { get; set; }
        public decimal P_TOTAL_PRICE { get; set; }
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
        public decimal _AM_TRANS { get; set; }
        public decimal Z { get; set; }
        public decimal P { get; set; }
        public decimal PT_TOTAL_PRICE { get; set; }
        public decimal _AM_PT { get; set; }
        public decimal TT_TOTAL_PRICE { get; set; }
        public decimal _AM_TT { get; set; }
        public decimal EXAM_KSK_TOTAL_PRICE { get; set; }
        public decimal _AM_EXAM_KSK { get; set; }
        public decimal GKSK_TOTAL_PRICE { get; set; }
        public decimal GKSK { get; set; }
        public decimal TOTAL_PRICE_PATIENT_BHYT { get; set; }
        public decimal BONE_DENSITY_TOTAL_PRICE { get; set; }
        public decimal _AM_BONE_DENSITY { get; set; }
        public decimal BONE_DENSITY_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_BONE_DENSITY_TOTAL_PRICE { get; set; }
        public decimal EEG_TOTAL_PRICE { get; set; }
        public decimal _AM_EEG { get; set; }
        public decimal EEG_QUOTA_EXPEND_TOTAL_PRICE { get; set; }
        public decimal DIFF_EEG_TOTAL_PRICE { get; set; }
        public decimal _AM_NSTH { get; set; }

        public int COUNT_TREATMENT { get; set; }

        public decimal CNHH_TOTAL_PRICE { get; set; }

        public decimal _AM_CNHH { get; set; }

        public decimal CNHH_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal CNHH_DENSITY_TOTAL_PRICE { get; set; }

        public decimal CLVT_TOTAL_PRICE { get; set; }

        public decimal CLVT_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal _AM_CLVT { get; set; }

        public decimal DIFF_CLVT_TOTAL_PRICE { get; set; }

        public decimal XQ_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal XQ_TOTAL_PRICE { get; set; }

        public decimal _AM_XQ { get; set; }

        public decimal DIFF_XQ_TOTAL_PRICE { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public decimal XNGX_TOTAL_PRICE { get; set; }

        public decimal _AM_XNGX { get; set; }

        public decimal XNGX_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_XNGX_TOTAL_PRICE { get; set; }

        public decimal CLVTYC_TOTAL_PRICE { get; set; }

        public decimal _AM_CLVTYC { get; set; }

        public decimal CLVTYC_QUOTA_EXPEND_TOTAL_PRICE { get; set; }

        public decimal DIFF_CLVTYC_TOTAL_PRICE { get; set; }

        public decimal GYC_TOTAL_PRICE { get; set; }

        public decimal _AM_GYC { get; set; }

        public decimal _AM_KYC { get; set; }

        public decimal KYC_TOTAL_PRICE { get; set; }

        public decimal? EXEMPTION { get; set; }
        public decimal BILL_SUM { get; private set; }


        public long TRANSACTION_TIME { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public long DEPARTMENT_ROOM_ID { get; set; }

        public string DEPARTMENT_ROOM_CODE { get; set; }

        public string DEPARTMENT_ROOM_NAME { get; set; }

        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }

        public long IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public string ICD_NAME { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }

        public long? PARENT_NUM_ORDER { get; set; }

        public long? CATEGORY_NUM_ORDER { get; set; }

        public string CATEGORY_NAME { get; set; }

        public long? TDL_FIRST_EXAM_ROOM_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? PTTT_GROUP_ID { get; set; }

        public long? SERVICE_REQ_STT_ID { get; set; }

        

        public Mrs00556RDO(Mrs00556RDO r, Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, Mrs00556Filter mrs00556Filter, List<HIS_EXECUTE_ROOM> listHistRoom, List<HIS_SERVICE> listHisService)
        {
           
           
        }

        public Mrs00556RDO()
        {
        }


        private void CategoryCode(long serviceId, List<HIS_SERVICE> listHisService)
        {
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
                if (service != null)
                {
                    if (string.IsNullOrEmpty(this.CATEGORY_CODE))
                    {
                        
                    }
                    else
                    {
                        this.CATEGORY_NUM_ORDER = 1000 + (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).NUM_ORDER;
                        this.CATEGORY_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        this.CATEGORY_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ParentCode(long serviceId, List<HIS_SERVICE> listHisService)
        {
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
                if (service != null)
                {
                    var parent = listHisService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                    if (parent != null)
                    {
                        this.PARENT_NUM_ORDER = parent.NUM_ORDER;
                        this.PARENT_SERVICE_CODE = parent.SERVICE_CODE;
                        this.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                    }
                    else
                    {
                        this.PARENT_NUM_ORDER = 1000 + (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).NUM_ORDER;
                        this.PARENT_SERVICE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        this.PARENT_SERVICE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        public decimal TRANS_TOTAL_PRICE_NEW { get; set; }

        public decimal _AM_TRANS_NEW { get; set; }
    }
}
