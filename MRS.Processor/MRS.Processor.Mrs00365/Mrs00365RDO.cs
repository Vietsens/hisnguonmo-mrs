using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00365
{
    public class Mrs00365RDO : V_HIS_SERE_SERV
    {
        public string INSTRUCTION_DATE_STR { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public long MALE_AGE { get; set; }
        public long FEMALE_AGE { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal? HEIN_PRICE { get; set; }
        public decimal? TOTAL_PATIENT_PRICE { get; set; }
        public decimal? TOTAL_HEIN_PRICE { get; set; }

    }
    public class LongClass
    {
       public long Value { get; set; }
    }
}
