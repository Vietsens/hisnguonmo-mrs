using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00591
{
    class Mrs00591RDO
    {
        public string NAME { get; set; }	
        public long EX_M { get; set; }	
        public long EX_F { get; set; }	
        public long TR_M { get; set; }
        public long TR_F { get; set; }
        public long PT_M { get; set; }	
        public long PT_F { get; set; }
        public long CT_M { get; set; }
        public long CT_F { get; set; }
    }
}
