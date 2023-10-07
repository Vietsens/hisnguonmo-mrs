using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00201
{
    public class Mrs00201Filter
    {
       public long DATE_FROM { get;  set;  }
       public long DATE_TO { get; set; }
       public long? KSK_CONTRACT_ID { get; set; }
       public bool? CHOOSE_TIME { get; set; }
       public List<long> SERVICE_REQ_STT_IDs { get; set; }
        //nếu tích vào thì không lấy thuốc vật tư. để mặc định là tích vào.
       public bool? IS_NOT_TAKE_MEMA { get; set; }
       public List<long> PATIENT_TYPE_IDs { get; set; }

       //chỉ lấy xét nghiệm, cận lâm sàng đã có kết quả.
       public bool? ALLOW_HAS_CONCLUDE { get; set; }

       public bool? IS_NOT_INDEX { get; set; }// không tách chỉ số xét nghiệm
    }
}
