using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00544
{
    public class Mrs00544RDO
    {
        public V_HIS_TREATMENT V_HIS_TREATMENT { get; set; }
        public V_HIS_HEIN_APPROVAL V_HIS_HEIN_APPROVAL { get; set; }
        public HIS_SERE_SERV HIS_SERE_SERV { get; set; }
        public string SERVICE_STT_DMBYT { get; set; }
        public string SERVICE_CODE_DMBYT { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string BRANCH_NAME { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public long? SERVICE_ID { get; set; }
        public string ROUTE_CODE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00544RDO()
        {
        }

        public Mrs00544RDO(HIS_SERE_SERV data)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.SERVICE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.SERVICE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.SERVICE_TYPE_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME;
                this.PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO);
            }
        }
    }
}
