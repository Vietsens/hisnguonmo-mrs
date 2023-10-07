using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport
{
    public class AbsProcessDelegate
    {
        private static ProcessMrs processMrs;
        public static ProcessMrs ProcessMrs
        {
            get
            {
                return processMrs;
            }
            set
            {
                processMrs = value;
            }
        }
    }
}
