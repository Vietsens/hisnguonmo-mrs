using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00637
{
    public class Mrs00637RDO
    {
        public string MEDI_MATE_TYPE_NAME { get;  set;  }
        public string MEDI_MATE_TYPE_CODE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public Decimal AMOUNT_MOBA_SUM { get;  set;  }
        public Decimal AMOUNT_EXP_SUM { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public Decimal VIR_TOTAL_PRICE { get;  set;  }
        public string AMOUNT_STR { get;  set;  }

        public Decimal Day1_Amount { get;  set;  }
        public Decimal Day2_Amount { get;  set;  }
        public Decimal Day3_Amount { get;  set;  }
        public Decimal Day4_Amount { get;  set;  }
        public Decimal Day5_Amount { get;  set;  }
        public Decimal Day6_Amount { get;  set;  }
        public Decimal Day7_Amount { get;  set;  }
        public Decimal Day8_Amount { get;  set;  }
        public Decimal Day9_Amount { get;  set;  }
        public Decimal Day10_Amount { get;  set;  }
        public Decimal Day11_Amount { get;  set;  }
        public Decimal Day12_Amount { get;  set;  }
        public Decimal Day13_Amount { get;  set;  }
        public Decimal Day14_Amount { get;  set;  }
        public Decimal Day15_Amount { get;  set;  }
        public Decimal Day16_Amount { get;  set;  }
        public Decimal Day17_Amount { get;  set;  }
        public Decimal Day18_Amount { get;  set;  }
        public Decimal Day19_Amount { get;  set;  }
        public Decimal Day20_Amount { get;  set;  }
        public Decimal Day21_Amount { get;  set;  }
        public Decimal Day22_Amount { get;  set;  }
        public Decimal Day23_Amount { get;  set;  }
        public Decimal Day24_Amount { get;  set;  }
        public Decimal Day25_Amount { get;  set;  }
        public Decimal Day26_Amount { get;  set;  }
        public Decimal Day27_Amount { get;  set;  }
        public Decimal Day28_Amount { get;  set;  }
        public Decimal Day29_Amount { get;  set;  }
        public Decimal Day30_Amount { get;  set;  }
        public Decimal Day31_Amount { get;  set;  }
    }
    public class DetailRDOMedicine
    {
        public decimal AMOUNT { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public long INTRUCTION_TIME { get; set; }
    }
    public class DetailRDOMaterial
    {
        public decimal AMOUNT { get; set; }
        public long MATERIAL_TYPE_ID { get; set; }
        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public long INTRUCTION_TIME { get; set; }
    }
}