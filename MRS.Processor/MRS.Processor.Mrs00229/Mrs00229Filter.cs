using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00229
{
    public class Mrs00229Filter
    {
        public long EXP_TIME_FROM { get;  set;  }
        public long EXP_TIME_TO { get;  set;  }
		public long? DEPARTMENT_ID { get;  set;  }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> EXP_MEST_STT_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        /// <summary>
        /// doi tuong quan nhan
        /// 1: bhyt quan nhan
        /// 2: bhyt than nhan
        /// 3: bhyt thuong
        /// 4: vien phi
        /// </summary>
        public List<long> INPUT_DATA_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }

        public bool? ADD_SALE { get; set; }
        
    }
}
