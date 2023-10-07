using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00250
{
    public class Mrs00250RDO
    {
        public string LOGINNAME { get;  set;  }
        public string USERNAME { get; set; }
        public Decimal AMOUNT { get; set; }
        public Decimal AMOUNT_BH { get; set; }
        public Decimal AMOUNT_VP { get; set; }
        public Decimal TOTAL_PRICE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string IS_BHYT { get; set; }
        public string INTRUCTION_TIME { get; set; }

        public Decimal AMOUNT_EXAM_SERVICE { get;  set;  }
        public Decimal AMOUNT_IN_TREAT { get;  set;  }

        public long PARENT_ID { get; set; }

        public Decimal AMOUNT_HAS_PRES { get; set; }

        public Decimal AMOUNT_HAS_PRES_BH { get; set; }
    }
}
