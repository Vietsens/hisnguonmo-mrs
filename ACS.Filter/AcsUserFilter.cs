using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Filter
{
    public class AcsUserFilter : FilterBase
    {
        public string LOGINNAME { get; set; }
        public string CN_WORD { get; set; }
        public string KEY_WORD { get; set; }
        public AcsUserFilter()
            : base()
        {
        }
    }
}
