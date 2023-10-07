using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00133
{
    /// <summary>
    /// Báo cáo chi tiết nhập xuất tồn theo các kho
    /// </summary>
    public class Mrs00133Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public bool? IS_MERGE { get; set; }
        public bool? IS_MERGE_STOCK { get; set; }
        public bool? IS_MERGE_STOCK_THROW_CHMS { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public List<long> MEDICINE_GROUP_IDs { set; get; }
        public short? NATIONAL_NAME { set; get; }//null:all; 1:VN; 0: Nước ngoài
        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public bool? IS_MEDICINE_GROUP { get; set; }

        public bool? IS_MERGE_CODE { get; set; }

        public Mrs00133Filter()
            : base()
        {

        }

        public long? PATIENT_TYPE_ID { get; set; }
    }
}
