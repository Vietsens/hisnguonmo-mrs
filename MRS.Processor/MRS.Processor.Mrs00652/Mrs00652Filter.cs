using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00652
{
    public class Mrs00652Filter
    {
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> END_ROOM_IDs { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public short? TIME_TYPE { get; set; }// null:duyet bhyt  | 0: vao vien | 1: ra vien |2: khoa vp
        public bool? IS_MERGE_TREATMENT { get; set; }
        public string MEDICINE_TYPE_CODE__HIVs { get; set; }
        public string ICD_CODE__HIVs { get; set; }

        public string CATEGORY_CODE__ARV { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public bool? IS_PROVINCE_FROM_MEDI_ORG { get;  set; }
    }
}
