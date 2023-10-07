using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00596
{
    public class Mrs00596Filter
    {
        public long IMP_TIME_FROM { get; set; }
        public long IMP_TIME_TO { get; set; }//THỜI GIAN NHẬP THUỐC
        public long? MEDI_STOCK_ID { get; set; }//LỌC THEO KHO

        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public bool? IS_MEDICINE_TD { get; set; }
        public bool? IS_MEDICINE_YHCT { get; set; }
        public bool? IS_MEDICINE_CPYHCT { get; set; }
        public bool? IS_MEDICINE_OTH { get; set; }

        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public List<long> BID_IDs { get; set; }

        public long? BID_CREATE_TIME_FROM { get; set; }//THỜI GIAN TẠO THẦU TỪ
        public long? BID_CREATE_TIME_TO { get; set; }//THỜI GIAN TẠO THẦU ĐẾN

        public string KEY_GROUP_IMP { get; set; }//key group dữ liệu chi tiết nhập

        public string KEY_HIDE_BEAN_0 { get; set; }//ẩn tồn bằng 0

        public string KEY_HIDE_AVAILABLE_0 { get; set; }//ẩn khả dụng 0
    }
}
