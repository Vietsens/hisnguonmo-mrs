using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00824
{
    public class Mrs00824RDO: HIS_SERE_SERV
    {
        public decimal PRICE_DV { get; set; }
        public string TREATMENT_CODE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ROOM_NAME { get; set; }
        public string EXAM_ROOM_NAME { get; set; }
        public long BRANCH_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
    }
}
