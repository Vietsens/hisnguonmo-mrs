using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00124
{
    /// <summary>
    /// Báo cáo tổng hợp số lượng thuốc vật tư xuất cho khoa
    /// </summary>
    class Mrs00124Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long MEDI_STOCK_ID { get;  set;  }

        /// <summary>
        /// chon thuoc, vat tu hoac ca hai
        /// null - chon ca hai (thuoc va vat tu)
        /// true - chi chon thuoc
        /// false - chi chon vat tu
        /// </summary>
        public bool? IsMedicineType { get;  set;  }

        public Mrs00124Filter()
            : base()
        {
        }
    }
}
