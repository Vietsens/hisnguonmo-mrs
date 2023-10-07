using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Filter
{
    public class AcsRoleFilter : FilterBase
    {
        public string KEY_WORD { get; set; }
        public long? APPLICATION_ID { get; set; }
        
        public AcsRoleFilter()
            : base()
        {
        }
    }
}
