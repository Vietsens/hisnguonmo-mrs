
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceMachineFilter : FilterBase
    {
        public List<long> SERVICE_IDs { get; set; }
        public List<long> MACHINE_IDs { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? MACHINE_ID { get; set; }

        public HisServiceMachineFilter()
            : base()
        {
        }
    }
}
