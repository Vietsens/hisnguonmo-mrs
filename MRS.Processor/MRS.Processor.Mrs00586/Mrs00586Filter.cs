using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00586
{
    public class Mrs00586Filter : HisTreatmentFilterQuery
    {

        public long? DEPARTMENT_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
    }
}