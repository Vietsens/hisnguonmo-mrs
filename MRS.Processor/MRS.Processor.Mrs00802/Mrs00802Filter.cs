using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00802
{
    public class Mrs00802Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? SUPPLIER_ID { get; set; }
        public List<long> SUPPLIER_IDs { get; set; }

        public List<long> MEDICINE_GROUP_IDs { set; get; }
        public long? MEDICINE_GROUP_ID { set; get; }

        public long? ANTICIPATE_ID { get; set; }
        public List<long> ANTICIPATE_IDs { get; set; }

        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }

        public string USE_TIME { set; get; }

    }
}
