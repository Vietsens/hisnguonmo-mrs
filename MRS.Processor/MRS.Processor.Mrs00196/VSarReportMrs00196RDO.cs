using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00196
{
    public class VSarReportMrs00196RDO : V_HIS_HEIN_APPROVAL
    {
        public string TYPE_CODE { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURG_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal MATERIAL_RATIO_PRICE { get; set; }
        public decimal SERVICE_RATIO_PRICE { get; set; }
        public decimal MEDICINE_RATIO_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal? TOTAL_DATE { get; set; }

        public int GROUP { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public VSarReportMrs00196RDO()
        {

        }

        public VSarReportMrs00196RDO(V_HIS_HEIN_APPROVAL Data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_HEIN_APPROVAL>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(Data)));
                }
                ProcessTypeCode();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessTypeCode()
        {
            try
            {
                TYPE_CODE = HEIN_CARD_NUMBER.Substring(0, 2);
                if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_1.Contains(TYPE_CODE))
                {
                    GROUP = 1;
                }
                else if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_2.Contains(TYPE_CODE))
                {
                    GROUP = 2;
                }
                else if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_3.Contains(TYPE_CODE))
                {
                    GROUP = 3;
                }
                else if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_4.Contains(TYPE_CODE))
                {
                    GROUP = 4;
                }
                else if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_5.Contains(TYPE_CODE))
                {
                    GROUP = 5;
                }
                else if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_6.Contains(TYPE_CODE))
                {
                    GROUP = 6;
                }
                else
                {
                    GROUP = 7;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
