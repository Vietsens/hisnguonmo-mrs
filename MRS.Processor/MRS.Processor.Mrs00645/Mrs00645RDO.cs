using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00645
{
    class Mrs00645RDO
    {
        public string HEIN_SERVICE_BHYT_ORDER { get; set; }
        public string HEIN_SERVICE_BHYT_CODE { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }

        public decimal RATIO_100 { get; set; }
        public decimal RATIO { get; set; }

        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }

        public decimal? ORIGINAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }

        public decimal? TOTAL_HEIN_PRICE { get; set; }

        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00645RDO() { }

        public Mrs00645RDO(HIS_SERE_SERV data)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.HEIN_SERVICE_BHYT_CODE = data.TDL_HEIN_SERVICE_BHYT_CODE ?? data.TDL_SERVICE_CODE;
                this.HEIN_SERVICE_BHYT_ORDER = data.TDL_HEIN_ORDER;
                this.HEIN_SERVICE_BHYT_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME ?? data.TDL_SERVICE_NAME;
                this.ORIGINAL_PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO);
                if (data.HEIN_LIMIT_PRICE == 0)
                {
                    data.HEIN_LIMIT_PRICE = null;
                }
                if (data.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    data.PRICE = 0;
                }
                this.PRICE = data.HEIN_LIMIT_PRICE.HasValue ? data.HEIN_LIMIT_PRICE.Value : data.PRICE * (1 + data.VAT_RATIO);
                //tyle theo xml
                decimal tyle = data.ORIGINAL_PRICE > 0 ? (data.HEIN_LIMIT_PRICE.HasValue ? (data.HEIN_LIMIT_PRICE.Value / (data.ORIGINAL_PRICE * (1 + data.VAT_RATIO))) : (data.PRICE / data.ORIGINAL_PRICE)) : 0;
                this.RATIO_100 = Math.Round(tyle * 100, 0);
                this.RATIO = Math.Round(tyle, 0);

                this.TOTAL_HEIN_PRICE = data.VIR_TOTAL_HEIN_PRICE;
            }
        }
    }
}
