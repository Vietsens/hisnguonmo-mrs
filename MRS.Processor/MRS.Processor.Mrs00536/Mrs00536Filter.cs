using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00536
{
    public class Mrs00536Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public short? IS_MEDI_MATE_CHEM { get; set; }//null:medi; 1:mate; 0:chem
        public long MEDI_STOCK_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public Mrs00536Filter()
            : base()
        {
        }
    }
}
