using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00455
{
    public class Mrs00455Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:vao vien, 2:chi dinh, 3:bat dau, 4:ket thuc, 5: ra vien, 6: thanh toan, 7: khoa vien phi, 8: Duyet giam dinh
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public string KEY_GROUP_SS { get; set; }
        public List<short> INPUT_DATA_ID_PRICE_TYPEs { get; set; } //nguồn tiền thanh toán: 1: Bệnh nhân không có thẻ bảo hiểm; 2: Bệnh nhân có thẻ BHYT (Bệnh nhân trả); 3: Bệnh nhân có thẻ BHYT (Bảo hiểm trả)
        public List<long> SERVICE_IDs { get; set; }
        public List<long> TDL_OTHER_PAY_SOURCE_IDs { get; set; }
        public List<long> EXIST_SERVICE_IDs { get; set; }
    }
}
