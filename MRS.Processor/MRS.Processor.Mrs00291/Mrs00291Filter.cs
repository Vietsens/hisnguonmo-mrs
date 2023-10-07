using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00291
{
    public class Mrs00291Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? SS_PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }

        public short? TIME_TYPE { get; set; }// null:vao vien  | 0: khoa vp | 1: ra vien

        public short? HEIN_SERVICE_TYPE { get; set; }// null:dich vu  | 0: vat tu | 1: thuoc

        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        //public bool? IS_ROUTE { get; set; }

    }
}
