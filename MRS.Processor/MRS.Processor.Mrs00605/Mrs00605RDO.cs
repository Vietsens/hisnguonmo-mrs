using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00605
{
    class Mrs00605RDO
    {
       public string NAME { get; set; }
       public decimal COUNT_EXAM { get; set; }
       public decimal COUNT_EXAM_FEMALE { get; set; }
       public decimal COUNT_EXAM_MALE { get; set; }
        public decimal COUNT_EXAM_BHYT { get; set; }	
        public decimal COUNT_EXAM_VP { get; set; }
        public decimal COUNT_EXAM_LESS6 { get; set; }
        public decimal COUNT_EXAM_OVER60 { get; set; }
        public decimal COUNT_EXAM_ETHANIC { get; set; }
        public decimal COUNT_EXAM_KINH { get; set; }
        public decimal COUNT_IN { get; set; }
        public decimal COUNT_IN_FEMALE { get; set; }
        public decimal COUNT_IN_MALE { get; set; }
        public decimal COUNT_IN_BHYT { get; set; }	
        public decimal COUNT_IN_VP { get; set; }	
        public decimal COUNT_IN_LESS6 { get; set; }
        public decimal COUNT_IN_OVER60 { get; set; }
        public decimal COUNT_IN_ETHANIC { get; set; }
        public decimal COUNT_IN_KINH { get; set; }
        public decimal COUNT_OUT { get; set; }
        public decimal COUNT_OUT_FEMALE { get; set; }
        public decimal COUNT_OUT_MALE { get; set; }
        public decimal COUNT_OUT_BHYT { get; set; }
        public decimal COUNT_OUT_VP { get; set; }
        public decimal COUNT_OUT_LESS6 { get; set; }
        public decimal COUNT_OUT_OVER60 { get; set; }
        public decimal COUNT_OUT_ETHANIC { get; set; }
        public decimal COUNT_OUT_KINH { get; set; }	
    }
}
