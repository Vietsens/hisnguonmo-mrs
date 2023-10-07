using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00590
{
    class Mrs00590RDO
    {
        public string COMMUNE_NAME { get; set; }
        public long COUNT_ICD_CODE__HHTs { get; set; }
        public long COUNT_ICD_CODE__VPs { get; set; }
        public long COUNT_ICD_CODE__CUs { get; set; }
        public long COUNT_ICD_CODE__LAs { get; set; }
        public long COUNT_ICD_CODE__LTs { get; set; }
        public long COUNT_ICD_CODE__QBs { get; set; }
        public long COUNT_ICD_CODE__TDs { get; set; }
        public long COUNT_ICD_CODE__TCs { get; set; }
        public long COUNT_ICD_CODE__VGs { get; set; }
        public long COUNT_ICD_CODE__ADs { get; set; }	
    }
}
