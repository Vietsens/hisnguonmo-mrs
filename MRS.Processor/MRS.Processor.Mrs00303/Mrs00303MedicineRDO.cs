using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00303
{
    class Mrs00303MedicineRDO
    {
        public long PARENT_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; } //mã nhóm cha
        public string MEDICINE_TYPE_NAME { get; set; } //tên nhóm cha
        public long MEDICINE_TYPE_ID { get; set; } //id con
    }
}
