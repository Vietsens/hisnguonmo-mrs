using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00019
{
    public class Mrs00019RDO
    {
        public string SERVICE_STT_DMBYT { get; set; }
        public string SERVICE_CODE_DMBYT { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string BRANCH_NAME { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public long? SERVICE_ID { get; set; }
        public string ROUTE_CODE { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string HEIN_APPROVAL_CODE { get; set; }
        public string END_CODE { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public string INTRUCTION_DATE_STR { get; set; }
        public string START_TIME_STR { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string AX_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal AMOUNT_CC { get; set; }

        public decimal AMOUNT_TNT { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string ICD_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public long TDL_HEIN_SERVICE_TYPE_ID { get; set; }
        public long HEIN_SERVICE_TYPE_ID { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }
        public string PROVINCE_TYPE { get; set; }// nội tỉnh/ ngoại tỉnh
        public string RIGHT_ROUTE_CODE { get; set; } // tuyến
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00019RDO()
        {
        }

        public Mrs00019RDO(HIS_SERE_SERV data)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.SERVICE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.SERVICE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.SERVICE_TYPE_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME;
                this.HEIN_SERVICE_TYPE_ID = data.TDL_HEIN_SERVICE_TYPE_ID??0;
                this.HEIN_PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO);
                this.PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO);
                this.TOTAL_HEIN_PRICE = (data.VIR_TOTAL_HEIN_PRICE ?? 0);
                this.BHYT_PAY_RATE = GetBHYTPayRate(data);
            }
        }

        private decimal GetBHYTPayRate(HIS_SERE_SERV s)
        {
            decimal result = 0;
            try
            {
                if (!s.HEIN_LIMIT_PRICE.HasValue || s.ORIGINAL_PRICE > s.HEIN_LIMIT_PRICE)
                {
                    result = 100;
                }
                else
                {
                    result = Math.Round((s.ORIGINAL_PRICE / s.HEIN_LIMIT_PRICE.Value) * 100, 0);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }
    }
}
