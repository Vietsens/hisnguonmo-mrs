using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00152
{
    public class Mrs00152Filter
    {
        public long DATE_FROM { get;  set;  }

        public long DATE_TO { get;  set;  }

        public long INVOICE_BOOK_ID { get;  set;  }//so hoa dơn


				public List<string> LOGINNAMEs { get;  set;  }//người tạo hóa đơn

        public Mrs00152Filter()
            : base()
        {

        }
    }
}
