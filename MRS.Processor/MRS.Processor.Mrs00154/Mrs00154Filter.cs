using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00154
{
    public class Mrs00154Filter
    {
        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }

        public List<long> IMP_MEDI_STOCK_IDs { get; set; }

        public bool? IS_MEDICINE { get; set; }

        public bool? IS_MATERIAL { get; set; }

        public short? INPUT_DATA_ID_PRICE_TYPE { get; set; }//Loại giá: 1. Giá xuất; 2. Giá bán

        public Mrs00154Filter()
            : base()
        {

        }
    }
}
