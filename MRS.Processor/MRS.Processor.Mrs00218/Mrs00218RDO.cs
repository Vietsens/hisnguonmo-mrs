using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00218
{
    public class Mrs00218RDO
    {
        public string MATERIAL_STT_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT_1 { get; set; }
        public string MATERIAL_TYPE_NAME_BYT { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string MATERIAL_QUYCACH_NAME { get; set; }
        public string MATERIAL_UNIT_NAME { get; set; }
        public decimal? IMP_PRICE { get; set; } // gia mua vao
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal VIR_TOTAL_HEIN_PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00218RDO()
        {

        }
    }
}
