using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00705
{
    class Mrs00705RDO : HIS_TREATMENT
    {
        public string PART_EXAM_EYESIGHT_LEFT { get; set; }
        public string PART_EXAM_EYESIGHT_RIGHT { get; set; }
        public string EYESIGHT_LEFT_NEW_1 { get; set; }
        public string EYESIGHT_RIGHT_NEW_1 { get; set; }
        public string EYESIGHT_LEFT_NEW_2 { get; set; }
        public string EYESIGHT_RIGHT_NEW_2 { get; set; }
        public string EYESIGHT_LEFT_NEW_3 { get; set; }
        public string EYESIGHT_RIGHT_NEW_3 { get; set; }
        public string NOTE { get; set; }
    }
}
