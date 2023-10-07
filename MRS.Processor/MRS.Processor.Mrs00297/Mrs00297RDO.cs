using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00297
{
    class Mrs00297RDO
    {
        public decimal AMOUNT { get;  set;  }
        public decimal RETURN_AMOUNT { get; set; }
        public decimal RETURN_PRICE { get; set; }
        public decimal IMP_PRICE { get;  set;  }
        public decimal INTEREST_PRICE { get;  set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }
        public decimal? PRICE { get;  set;  }
        public decimal? TOTAL_IMP_PRICE { get;  set;  }
        public decimal? TOTAL_RETURN_PRICE { get; set; }
        public decimal? TOTAL_PRICE { get;  set;  }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? VAT_RATIO { get; set; }

        public long TYPE_ID { get;  set;  }
        public string TYPE_NAME { get;  set;  }
        public decimal? TYPE_TOTAL_IMP_PRICE { get;  set;  }
        public decimal? TYPE_TOTAL_PRICE { get;  set;  }
        public decimal? TYPE_TOTAL { get;  set;  }

        public decimal TOTAL_TYPE_IMP_PRICE { get;  set;  }
        public decimal TOTAL_TYPE_PRICE { get;  set;  }
        public decimal TOTAL_TYPE { get;  set;  }

        public long EXP_MEST_TYPE_ID { get; set; }
        public string EXP_MEST_TYPE_CODE { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }

        public long IMP_MEST_TYPE_ID { get; set; }
        public string CONCENTRA { get; set; }
        public string MEDICINE_NAME { get; set; }

        public long? EXP_TIME { get; set; }

        public string EXP_TIME_STR { get; set; }

        public string EXP_MEST_CODE { get; set; }

        public string IMP_MEST_CODE { get; set; }

        public decimal IMP_VAT_RATIO { get; set; }

        public decimal PRICE_1 { get; set; }

        public decimal VAT_RATIO_1 { get; set; }

        public decimal TOTAL_PRICE_1 { get; set; }

        public decimal? TYPE_TOTAL_PRICE_1 { get; set; }

        public decimal INTEREST_PRICE_1 { get; set; }

        public decimal? TYPE_TOTAL_1 { get; set; }

        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public decimal EXEMPTION { get; set; }
    }
    class EXT_INFO
    {
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public decimal EXEMPTION { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }

        public string EXP_MEST_CODE { get; set; }
    }
}
