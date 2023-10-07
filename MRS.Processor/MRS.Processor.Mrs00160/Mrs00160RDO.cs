using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00160
{
    public class Mrs00160RDO
    {
        public decimal DEPARTMENT_0 { get;  set;  }
        public decimal DEPARTMENT_1 { get;  set;  }
        public decimal DEPARTMENT_2 { get;  set;  }
        public decimal DEPARTMENT_3 { get;  set;  }
        public decimal DEPARTMENT_4 { get;  set;  }
        public decimal DEPARTMENT_5 { get;  set;  }
        public decimal DEPARTMENT_6 { get;  set;  }
        public decimal DEPARTMENT_7 { get;  set;  }
        public decimal DEPARTMENT_8 { get;  set;  }
        public decimal DEPARTMENT_9 { get;  set;  }
        public decimal DEPARTMENT_10 { get;  set;  }
        public decimal DEPARTMENT_11 { get;  set;  }
        public decimal DEPARTMENT_12 { get;  set;  }
        public decimal DEPARTMENT_13 { get;  set;  }
        public decimal DEPARTMENT_14 { get;  set;  }
        public decimal DEPARTMENT_15 { get;  set;  }
        public decimal DEPARTMENT_16 { get;  set;  }
        public decimal DEPARTMENT_17 { get;  set;  }
        public decimal DEPARTMENT_18 { get;  set;  }
        public decimal DEPARTMENT_19 { get;  set;  }
        public decimal DEPARTMENT_20 { get;  set;  }
        public decimal DEPARTMENT_21 { get;  set;  }
        public decimal DEPARTMENT_22 { get;  set;  }
        public decimal DEPARTMENT_23 { get;  set;  }
        public decimal DEPARTMENT_24 { get;  set;  }
        public decimal DEPARTMENT_25 { get; set; }

        public decimal MEDI_STOCK_0 { get; set; }
        public decimal MEDI_STOCK_1 { get; set; }
        public decimal MEDI_STOCK_2 { get; set; }
        public decimal MEDI_STOCK_3 { get; set; }
        public decimal MEDI_STOCK_4 { get; set; }
        public decimal MEDI_STOCK_5 { get; set; }
        public decimal MEDI_STOCK_6 { get; set; }
        public decimal MEDI_STOCK_7 { get; set; }
        public decimal MEDI_STOCK_8 { get; set; }
        public decimal MEDI_STOCK_9 { get; set; }
        public decimal MEDI_STOCK_10 { get; set; }
        public decimal MEDI_STOCK_11 { get; set; }
        public decimal MEDI_STOCK_12 { get; set; }
        public decimal MEDI_STOCK_13 { get; set; }
        public decimal MEDI_STOCK_14 { get; set; }
        public decimal MEDI_STOCK_15 { get; set; }
        public decimal MEDI_STOCK_16 { get; set; }
        public decimal MEDI_STOCK_17 { get; set; }
        public decimal MEDI_STOCK_18 { get; set; }
        public decimal MEDI_STOCK_19 { get; set; }
        public decimal MEDI_STOCK_20 { get; set; }
        public decimal MEDI_STOCK_21 { get; set; }
        public decimal MEDI_STOCK_22 { get; set; }
        public decimal MEDI_STOCK_23 { get; set; }
        public decimal MEDI_STOCK_24 { get; set; }
        public decimal MEDI_STOCK_25 { get; set; }

        public long SERVICE_TYPE_ID { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public string MEDICINE_TYPE_NAME { get; set; }

        public long SERVICE_ID { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }

        public string MEDICINE_LINE_CODE { get; set; }

        public string MEDICINE_LINE_NAME { get; set; }

        public long? MEDICINE_GROUP_ID { get; set; }

        public string MEDICINE_GROUP_CODE { get; set; }

        public string MEDICINE_GROUP_NAME { get; set; }

        public decimal EXP_AMOUNT { get; set; }

        public decimal MOBA_EXP_AMOUNT { get; set; }

        public string MATERIAL_TYPE_CODE { get; set; }

        public string MEDICINE_TYPE_CODE { get; set; }

        public Dictionary<string, decimal> DIC_DEPARTMENT_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_MEDI_STOCK_AMOUNT { get; set; }
    }

    public class KeyDepartmentName
    {

        public string DEPARTMENT_KEY { get; set; }

        public HIS_DEPARTMENT HIS_DEPARTMENT { get; set; }
    }
    public class KeyMediStockName
    {

        public string MEDI_STOCK_KEY { get; set; }

        public V_HIS_MEDI_STOCK HIS_MEDI_STOCK { get; set; }
    }
}
