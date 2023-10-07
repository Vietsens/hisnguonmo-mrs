using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00541
{
    class Mrs00541RDO : HIS_SERVICE_REQ
    {
        public string FEMALE_YEAR { get; set; }
        public string MALE_YEAR { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_CARD_ADDRESS { get; set; }
        public long? HEIN_CARD_FROM_TIME { get; set; }
        public long? HEIN_CARD_TO_TIME { get; set; }
        public string TREATMENT_METHOD { get; set; }
        public HIS_DHST DHST { get; set; }
        public V_HIS_PATIENT PATIENT { get; set; }
        public V_HIS_TREATMENT_4 TREATMENT { get; set; }
    }

    class Mrs00541SereServRDO
    {
        public long? TDL_TREATMENT_ID { get; set; }
        public V_HIS_SERE_SERV_2 DVKT { get; set; }
        public V_HIS_SERE_SERV_2 MEDI_MATE { get; set; }
    }

    public class TreatmentId
    {
        public long ID { get; set; }
    }
}
