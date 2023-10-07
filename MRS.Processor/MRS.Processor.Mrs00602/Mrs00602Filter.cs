using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00602
{
    public class Mrs00602Filter
    {
        public long FEE_LOCK_TIME_FROM { get; set; }
        public long FEE_LOCK_TIME_TO { get; set; }
        public bool? IS_TREAT { get; set; }
        public bool? THROW_OUTTREAT_SERVICE { get; set; }
        public bool? THROW_TREATMENT_TYPE_EXAM { get; set; }
        public bool? HAS_EXPEND { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public long? CHOOSE_TIME_FILTER { get; set; }
        public List<string> REQUEST_LOGINNAME { set; get; }
        public bool? IS_NOT_BHYT { get; set; }
        public bool? IS_PAY { get; set; }

        public short? STATUS_TREATMENT { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public bool IS_SPLIT_ROUTE { get; set; }
        public bool? IS_PAUSE_AND_ACTIVE { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        //thêm điều kiện lọc hình thức thanh toán
        public List<long> PAY_FORM_IDs { get; set; }

        //thêm điều kiện lọc lấy tất cả bệnh nhân
        public bool? IS_ALL_STATUS { get; set; }

        //thêm điều kiện lọc lấy dịch vụ chỉ định ở buồng bệnh nội trú
        public bool? IS_FROM_BED { get; set; }

        //thêm điều kiện lọc lấy dịch vụ không chỉ định ở buồng bệnh nội trú
        public bool? IS_NOT_FROM_BED { get; set; }

        //diện thanh toán
        public short? INPUT_DATA_ID_BILL_TREAT_TYPE { get; set; } //1:Thanh toán ngoại trú; 2: Thanh toán nội trú
      

    }
}
