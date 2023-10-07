using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00220
{
    public class Mrs00220RDO
    {
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_CODE_DMBYT { get; set; }
        public string MEDICINE_STT_DMBYT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }

        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00220RDO() { }
    }
}
