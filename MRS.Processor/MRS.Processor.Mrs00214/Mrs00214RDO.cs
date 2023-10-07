using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00214
{
    public class Mrs00214RDO : V_HIS_HEIN_APPROVAL
    {
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long? TOTAL_DATE { get; set; }

        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }

        //TheoThoi gian vao
        public string IN_TIME_STR { get; set; }
        public long? MAIN_DAY { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00214RDO()
        {

        }

        public Mrs00214RDO(V_HIS_HEIN_APPROVAL heinApproval)
        {
            try
            {
                if (heinApproval != null)
                {
                    System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<V_HIS_HEIN_APPROVAL>();
                    foreach (var item in pis)
                    {
                        item.SetValue(this, item.GetValue(heinApproval));
                    }
                    this.VIR_PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
                    SetExtendField();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField()
        {
            if (this.TDL_PATIENT_DOB > 0)
            {
                try
                {
                    if (this.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.DOB_MALE = long.Parse((this.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    else if (this.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.DOB_FEMALE = long.Parse((this.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }
    }
}
