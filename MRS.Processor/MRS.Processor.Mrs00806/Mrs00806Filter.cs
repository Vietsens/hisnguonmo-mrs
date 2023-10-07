using IMSys.DbConfig.HIS_RS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00806
{
    public class Mrs00806Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { set; get; }
        public List<long> MEDI_STOCK_IDs  {set;get;}
    }
}
