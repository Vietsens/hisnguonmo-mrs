using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00274
{
    public class Mrs00274Filter
    {
         public long TIME_FROM { get;  set;  }
         public long TIME_TO { get; set; }
         public List<long> MEDI_STOCK_BUSINESS_IDs { get; set; }
         public List<long> MEDICINE_TYPE_IDs { get; set; }
         public List<long> MATERIAL_TYPE_IDs { get; set; }

         public string LOGINNAME_SALE { get; set; }

         public string BIOLOGY_PRODUCTs { get; set; }

         public string REQUEST_LOGINNAME { get; set; }

         public List<long> PAY_FORM_IDs { get; set; }

        public List<long> NO_BILL_PAY_FORM_IDs { get; set; }

        public long INPUT_DATA_ID_TIME_TYPE { get; set; } //1. THỜI GIAN XUẤT  2. THỜI GIAN TẠO

        public string CASHIER_LOGINNAME { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public List<long> SERVICE_TYPE_IDs { get; set; }

        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
				
    }
}
	