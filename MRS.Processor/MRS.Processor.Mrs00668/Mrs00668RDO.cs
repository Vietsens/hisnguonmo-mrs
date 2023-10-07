using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00668
{
    public class BILL
    {
        public string BILL_TEMPLATE_CODE { get; set; }
        public string BILL_ACCOUNT_BOOK_CODE { get; set; }
        public string BILL_ACCOUNT_BOOK_NAME { get; set; }
        public string BILL_SYMBOL_CODE { get; set; }
        public long BILL_NUM_ORDER { get; set; }

        public long TREATMENT_ID { get; set; }
    }
    public class TREATMENT
    {
        public string TDL_TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public long TREATMENT_ID { get; set; }

        public BILL BILL1 { get; set; }
        public BILL BILL2 { get; set; }
        public BILL BILL3 { get; set; }

        public TREATMENT()
        {
           BILL1 = new BILL();

             BILL2 = new BILL();

            BILL3 = new BILL();

        }
    }
    public class DEPOSIT
    {
        public string DEPOSIT_TEMPLATE_CODE { get; set; }
        public string DEPOSIT_ACCOUNT_BOOK_CODE { get; set; }
        public string DEPOSIT_ACCOUNT_BOOK_NAME { get; set; }
        public string DEPOSIT_SYMBOL_CODE { get; set; }
        public long DEPOSIT_NUM_ORDER { get; set; }
        public long DEPOSIT_TRANSACTION_TIME { get; set; }
        public decimal DEPOSIT_AMOUNT { get; set; }

        public long TREATMENT_ID { get; set; }
    }
}
