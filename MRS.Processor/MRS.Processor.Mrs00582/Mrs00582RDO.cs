using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00582
{
    public class Mrs00582RDO
    {
        public string SERVICE_NAME { get; set; }
        public string CATEGORY_NAME { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public decimal AMOUNT_VP_NT { get; set; }

        public decimal AMOUNT_VP_NGT { get; set; }

        public decimal AMOUNT_BH_NT { get; set; }

        public decimal AMOUNT_BH_NGT { get; set; }

        public decimal AMOUNT_CA_NT { get; set; }

        public decimal AMOUNT_CA_NGT { get; set; }

        public decimal AMOUNT_NN_NT { get; set; }

        public decimal AMOUNT_NN_NGT { get; set; }

        public decimal AMOUNT_KSK { get; set; }

        public decimal AMOUNT_CHILD_LESS5 { get; set; }

        public decimal AMOUNT_LESS5_NT { get; set; }

        public decimal AMOUNT_LESS5_NGT { get; set; }

        public decimal AMOUNT_OLDSTER_MORE60 { get; set; }

        public decimal AMOUNT_FEMALE { get; set; }

        public decimal AMOUNT_ETHNIC { get; set; }


        public string PR_SERVICE_NAME { get; set; }

        public string PR_SERVICE_CODE { get; set; }

        public decimal AMOUNT_NT { get; set; }

        public decimal AMOUNT_NGT { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
    }
    public class SereServDO
    {
        public long SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public decimal AMOUNT { get; set; }


        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long IN_TIME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? SERVICE_REQ_ID { get; set; }

        public decimal? PRICE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }

        public long REPORT_TYPE_CAT_ID { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_ETHNIC_NAME { get; set; }

        public long REQUEST_DEPARTMENT_ID { get; set; }

        public long EXECUTE_DEPARTMENT_ID { get; set; }
    }
}
