using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00477
{
    public class Mrs00477Filter : HisServiceReqFilter
    {
        public string CATEGORY_CODE__XQ { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; } 
    }
}
