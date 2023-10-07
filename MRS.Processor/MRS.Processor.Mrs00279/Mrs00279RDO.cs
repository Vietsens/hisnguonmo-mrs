using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00279
{
    public class Mrs00279RDO
    {
        public string MATERIAL_STT_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT_1 { get; set; }
        public string MATERIAL_TYPE_NAME_BYT { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string MATERIAL_QUYCACH_NAME { get; set; }
        public string MATERIAL_UNIT_NAME { get; set; }
        public decimal MATERIAL_PRICE { get; set; } // gia mua vao
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
    }
}
