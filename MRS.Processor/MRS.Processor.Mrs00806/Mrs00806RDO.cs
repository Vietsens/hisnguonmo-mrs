using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00806
{
    public class Mrs00806RDO
    {
        public long EXP_MEST_ID { set; get; }
        public long EXP_MEST_MEDICINE_ID { set; get; }
        public decimal EXP_MEST_AMOUNT { set; get; }
        public decimal EXP_MEST_PRICE { set; get; }
        public decimal TOTAL_PRICE { set; get; }
        public long SERVICE_UNIT_ID { set; get; }
        public string SERVICE_UNIT_NAME { set; get; }// đơn vị tính
        public long PATIENT_ID { set; get; }
        public string PATIENT_CODE { set; get; }
        public string PATIENT_NAME { set; get; }
        public string PATIENT_ADDRESS { set; get; }// địa chỉ bệnh nhân
        public string MEDICINE_NAME { set; get; }
        public long TREATMET_ID { set; get; }
        public string EXP_MEST_CODE { set; get; }// mã phiếu xuất
        public long FEE_LOCK_TIME { set; get; }// ngày khóa viện phí
        public string MEDICINE_GROUP_NAME { set; get; }
        public string MEDICINE_GROUP_CODE { set; get; }
    }
}
