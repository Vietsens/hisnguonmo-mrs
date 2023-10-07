using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00553
{
    public class Mrs00553Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public bool? IS_BLOOD { get; set; }
        public long? BID_ID { get; set; }

        /*Tạm thời bỏ do chưa có điều kiện lọc các hóa chất*/
        //public List<long> SERVICE_TYPE_IDs { get; set; } //đã giới hạn chỉ lấy thuốc hoặc vật tư
        public List<long> SERVICE_IDs { get; set; }
        //public List<long> CHEMICAL_SUBSTANCE_TYPE_IDs { get; set; } //hóa chất

        public List<long> IMP_SOURCE_IDs { get; set; }
        public string KEY_GROUP_EXP { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        
        public List<long> SUPPLIER_IDs { get; set; }

        public string KEY_ORDER_IMP { get; set; } // key thay đổi trường sắp xếp
    }
}
