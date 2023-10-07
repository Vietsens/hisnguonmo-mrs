
using System.Collections.Generic;
namespace TYT.Filter
{
    public class TytNervesFilter : FilterBase
    {
        public TytNervesFilter()
            : base()
        {
        }

        public string BRANCH_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PERSON_CODE { get; set; }
        public List<string> PATIENT_CODEs { get; set; }
        public List<string> PERSON_CODEs { get; set; }
    }
}
