
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExecuteRoleUserFilter : FilterBase
    {
        public long? EXECUTE_ROLE_ID { get; set; }
        public List<long> EXECUTE_ROLE_IDs { get; set; }
        public string LOGINNAME__EXACT { get; set; }

        public HisExecuteRoleUserFilter()
            : base()
        {
        }
    }
}
