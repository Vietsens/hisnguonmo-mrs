using Inventec.Core;
using MRS.Processor.Mrs00274;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00653
{
    class Mrs00653Processor : Mrs00274Processor
    {
        CommonParam paramGet = new CommonParam();
        public Mrs00653Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
    }
}
