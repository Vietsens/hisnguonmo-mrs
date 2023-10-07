using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00472
{
    public class Mrs00472RDO
    {
        public decimal TREATMENT_EXAM { get;  set;  }         // bn khám (kết thúc là khám)
        public decimal TOTAL_EXAM { get;  set;  }             // tổng số lượt khám
        public decimal HEIN_RIGHT { get;  set;  }             // đúng tuyến
        public decimal HEIN_LEFT { get;  set;  }              // trái tuyến
        public decimal OUT_PROVINCE { get;  set;  }           // tỉnh khác
        public decimal PATY_FEE { get;  set;  }               // viện phí
        public decimal INTERCONNECTED { get;  set;  }         // thông tuyến

        public decimal TREATMENT_OUT { get;  set;  }          // số bn điều trị ngoại trú
        public decimal TREATMENT_OUT_DATE { get;  set;  }     // số ngày điều trị ngoại trú
        public decimal TREATMENT_IN { get;  set;  }           // số bệnh nhân nội trú
        public decimal TREATMENT_IN_DATE { get;  set;  }      // số bệnh nhân ngoại trú

        public decimal TRAN_PATI { get;  set;  }              // chuyển viện
        public decimal TRAN_PATI_HIGHT { get;  set;  }        // chuyển leen tuyến trên
            
        public decimal TOTAL_SURG { get;  set;  }             // phẫu thuật
        public decimal SURG_TYPE_1 { get;  set;  }            // pt loại 1
        public decimal SURG_TYPE_2 { get;  set;  }            // pt loại 2
        public decimal SURG_TYPE_3 { get;  set;  }            // pt loại 3

        public decimal SURG_NS { get;  set;  }                // pt nội soi
        public decimal SURG_VP { get;  set;  }                // pt vi phân

        public Mrs00472RDO() { }
    }

    public class TREATMENT_LOG_472
    {
        public long TREATMENT_ID { get;  set;  }
        public long TREATMENT_TYPE_ID { get;  set;  }
        public long IN_TIME { get; set; }
        public long OUT_TIME { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
    }
}
