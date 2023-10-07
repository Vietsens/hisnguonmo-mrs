using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisExamSereDireFilter : FilterBase
    {
        public long? SERVICE_REQ_ID { get; set; }
        public long? DISEASE_RELATION_ID { get; set; }

        public HisExamSereDireFilter()
            : base()
        {
        }
    }
}
