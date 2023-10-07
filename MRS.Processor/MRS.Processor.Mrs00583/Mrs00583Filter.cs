using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00583
{
    public class Mrs00583Filter : HisServiceReqFilter
    {
        public List<string> SERVICE_CODE__TEETHs { get; set; }
        public string CATEGORY_CODE__XQ { get; set; } 
    }
}
