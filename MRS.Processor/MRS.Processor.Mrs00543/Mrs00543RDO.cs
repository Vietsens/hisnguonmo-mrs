using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00543
{
    public class Mrs00543RDO
    {
        public V_HIS_TREATMENT V_HIS_TREATMENT { get; set; }
        public V_HIS_HEIN_APPROVAL V_HIS_HEIN_APPROVAL { get; set; }
        public V_HIS_SERE_SERV_3 V_HIS_SERE_SERV_3 { get; set; }
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_CODE_DMBYT { get; set; }
        public string MEDICINE_STT_DMBYT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }

        public long? SERVICE_ID { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }
        public string REGISTER_NUMBER { get; set; }

        public string MEDICINE_DUONGDUNG_CODE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
    }
}
