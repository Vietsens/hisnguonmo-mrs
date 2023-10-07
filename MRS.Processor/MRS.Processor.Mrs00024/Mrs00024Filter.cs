using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00024
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo benh nhan
    /// </summary>
    class Mrs00024Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public bool? CHOOSE_TIME { get; set; }//null:transactionTime;true:fee_lock_time;false:transaction_time

        public long? PATIENT_TYPE_ID { get;  set;  }
        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<long> SERVICE_TYPE_IDs { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public List<long> EXACT_EXECUTE_ROOM_IDs { get; set; }

        public Mrs00024Filter()
            : base()
        {
        }
    }
}
