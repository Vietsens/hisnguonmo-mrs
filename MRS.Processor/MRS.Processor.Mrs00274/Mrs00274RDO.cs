using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00274
{
    public class Mrs00274RDO : V_HIS_EXP_MEST
    {
        public Mrs00274RDO()
        {

        }
        public Mrs00274RDO(V_HIS_EXP_MEST sale)
        {
            if (sale != null)
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_EXP_MEST>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(sale)));
                }
            }
        }
        public Mrs00274RDO(Mrs00274RDO sale)
        {
            if (sale != null)
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00274RDO>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(sale)));
                }
            }
        }
        public decimal PRICE { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal VAT_RATIO { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal FUND { get; set; }
        public string MAME_TYPE_CODE { get; set; }
        public string MAME_TYPE_NAME { get; set; }
        public string TIME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string PACKAGE_NUMBER { get; set; }

        public string TRANSACTION_CODE { get; set; }
        public long TRANSACTION_NUM_ORDER { get; set; }
        public string EINVOICE_NUM_ORDER { get; set; }
        public long EINVOICE_TIME { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public short? IS_BIOLOGY_PRODUCT { get; set; }

        public string REQUEST_ROOM_CODE { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_CODE_DMBYT { get; set; }
        public string MEDICINE_STT_DMBYT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_CODE { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }

        public string SERVICE_REQ_CODE { get; set; }

        public string CONCENTRA { get; set; }
        public short? IS_PRINTED { get; set; }
        public string THUOC_VATTU { get; set; }

        public decimal TH_AMOUNT { get; set; }
        public decimal TH_PRICE { get; set; }
        public decimal REAL_PRICE { get; set; }

        public decimal PRICE_1 { get; set; }

        public long REQUEST_DEPARTMENT_ID { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public decimal? EXP_PRICE { get; set; }

        public decimal? EXP_TOTAL_PRICE { get; set; }

        public string IMP_MEST_CODE { get; set; }

        public decimal VAT_RATIO_1 { get; set; }

        public short HAS_BILL { get; set; }

        public Dictionary<string, decimal> DIC_MTY_AMOUNT { get; set; }
    }

    public class PrintLogUnique
    {
        public string UNIQUE_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
    }
}
