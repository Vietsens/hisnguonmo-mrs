using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00503
{
    class Mrs00503RDO : V_HIS_HEIN_APPROVAL
    {
        public int APPROVAL_MONTH { get; set; }
        public decimal AMOUNT { get; set; }
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
        public decimal BED_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00503RDO() { }

        public Mrs00503RDO(V_HIS_HEIN_APPROVAL Data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_HEIN_APPROVAL>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(Data)));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
