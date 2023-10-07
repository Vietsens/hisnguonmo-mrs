using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00091
{
  /// <summary>
        /// Báo cáo tổng hợp nhập xuất tồn thuốc theo kho và ngày
        /// </summary>
        public class Mrs00091Filter : FilterBase
        {
            public long TIME_FROM { get;  set;  }
            public long TIME_TO { get;  set;  }

            public long? MEDI_STOCK_ID { get;  set;  }
            /// <summary>
            /// nhập, xuất, tồn hoặc cả ba
            /// null - cả nhập xuất tồn
            /// 0 - chỉ nhập
            /// 1 - chỉ xuất
            /// 2 - chỉ tồn
            /// </summary>
            public bool? IS_EXP { get;  set;  }
            public bool? IS_IMP { get;  set;  }
            public bool? IS_LEFT { get;  set;  }
            public Mrs00091Filter()
                : base()
            {

            }
        }
}
