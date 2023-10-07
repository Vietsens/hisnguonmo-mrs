using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00385
{
    public class Mrs00385Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> PARENT_MATERIAL_TYPE_IDs { get; set; }

        public bool? IS_MOBA_ON_TIME { get; set; }

        public short? IS_BUSINESS { get; set; }//null:tat ca; 1:Thuoc kinh doanh; 0:Thuoc khong kinh doanh

        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
    }
}
