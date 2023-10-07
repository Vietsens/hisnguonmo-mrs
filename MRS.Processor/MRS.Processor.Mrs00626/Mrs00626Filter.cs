using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00626
{
    public class Mrs00626Filter : HisTreatmentFilterQuery
    {
        public List<long> DEPARTMENT_IDs { get; set; }
    }
}
