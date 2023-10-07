using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00595
{
    public class Mrs00595Filter : HisTreatmentFilterQuery
    {
        public string CATEGORY_CODE__ECG { get; set; }
        public string CATEGORY_CODE__BRAIN { get; set; }
        public string CATEGORY_CODE__XRAY { get; set; }
    }
}