using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00803
{
    public class Mrs00803Filter
    {
        public Mrs00803Filter() { }
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> SERVICE_IDs { set; get; }
        //public List<long> SERVICE_TYPE_IDs { set; get; }
        //public long MATERIAL_TYPE_ID { set; get; }
    }
}
