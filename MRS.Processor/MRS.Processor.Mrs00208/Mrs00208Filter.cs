using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00208
{
    public class Mrs00208Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }

        public bool? TRUE_FALSE { get; set; }//Thời gian y lệnh: sai;

        public List<long> DEPARTMENT_IDs { get; set; }
        public long? EXAM_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public bool? ADD_SALE { get; set; }

        public long? REPORT_TYPE_CAT_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
    }
}
