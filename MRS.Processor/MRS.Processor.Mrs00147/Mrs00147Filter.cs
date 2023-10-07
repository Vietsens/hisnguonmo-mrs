using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00147
{
    /// <summary>
    /// Báo cáo sử dụng kháng sinh
    /// </summary>
    class Mrs00147Filter
    {
        public long? MEDI_STOCK_ID { get;  set;  }
        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
        public long? EXP_MEST_STT_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public string EMT_LIMIT_CODE { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MEDI_STOCK_NOT_BUSINESS_IDs { get; set; }
        public long? SERVICE_UNIT_ID { get; set; }
        public List<long> SERVICE_UNIT_IDs { get; set; }
        public long? MEDICINE_USE_FORM_ID { get; set; }
        public List<long> MEDICINE_USE_FORM_IDs { get; set; }
        public string MUF_LIMIT_CODE { get; set; }
        public bool? IS_ALL_ACTIVE_INGR_BHYT { get; set; }
        public bool? IS_THROW_ATC_NULL { get; set; }
        public bool? IS_MOBA_ON_TIME { get; set; }
    }
}
