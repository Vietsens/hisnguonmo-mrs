using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00328
{
    public class Mrs00328Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public bool? IS_MOBA_ON_TIME { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public short? IS_BUSINESS { get; set; }//null:tat ca; 1:Thuoc kinh doanh; 0:Thuoc khong kinh doanh
        public long? BRANCH_ID { get; set; }
    }
}
