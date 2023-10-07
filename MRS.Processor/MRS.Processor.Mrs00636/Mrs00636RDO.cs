using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00636
{
    //public class Mrs00636RDO
    //{
    //    public int NUM_ORDER { get; set; }

    //    public string CONTENT_NAME { get; set; }

    //    public decimal CONTENT_SUM { get; set; }

    //    public Dictionary<string,decimal> DIC_CONTENT { get; set; }
    //}
    public class DepartmentCountRDO
    {
        public long DEPARTMENT_ID { get; set; }

        public decimal COUNT { get; set; }

        public decimal TOTAL_DATE { get; set; }

        public long TREATMENT_TYPE_ID { get; set; }

        public string CATEGORY_CODE { get; set; }
    }
}
