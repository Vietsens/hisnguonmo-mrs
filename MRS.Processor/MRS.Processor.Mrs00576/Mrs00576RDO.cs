using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00576
{
    public class Mrs00576RDO : HIS_BABY
    {
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_ETHNIC_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS{ get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }

        public Mrs00576RDO() { }
    }
}
