using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00658
{
    public class Mrs00658Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public short? IS_NOT_FEE { get; set; }//null:all; 1:khong lay; 0:co
        public short? IS_NOT_GATHER_DATA { get; set; }//null:all; 1:khong lay; 0:co

        public short? IS_PT_TT { get; set; }//null:all; 1:PT; 0: TT
        public short? IS_NT_NGT { get; set; }//null:all; 1:NT; 0:NGT
        public short? IS_NT_NGT_0 { get; set; }//null:all; 1:NT; 0:NGT

        public string PTTT_GROUP_CODE__TTs { get; set; }

        public string PTTT_GROUP_CODE__PTs { get; set; }
    }
}
