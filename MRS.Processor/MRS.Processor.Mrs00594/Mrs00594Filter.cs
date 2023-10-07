using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00594
{
    public class Mrs00594Filter : HisServiceReqFilter
    {
        public bool? IS_TREAT { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
        
    }
}
