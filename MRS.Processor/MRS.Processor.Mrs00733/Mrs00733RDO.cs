using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00733
{
    class Mrs00733RDO
    {
        
        public string MEDICINE_TYPE_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string ACTIVE_INGREDIENT_CODE { get; set; } //mã hoạt chất xung đột
        public string ACTIVE_INGREDIENT_NAME { get; set; } //tên hoạt chất xung đột
        public long? GRADE { get; set; } //mức độ
        public string DESCRIPTION { get; set; } //mô tả
    }
}
