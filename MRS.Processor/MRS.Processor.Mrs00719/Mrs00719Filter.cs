using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00719
{
    class Mrs00719Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; }
        public Mrs00719Filter()
            : base()
        {

        }
    }
}
