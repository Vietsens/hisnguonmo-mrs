using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00188
{
    public class Mrs00188Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? PATIENT_TYPE_ID { get; set; } //doi tuong thanh toan
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public bool? IS_ADD_INVENTORY { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
    }
}
