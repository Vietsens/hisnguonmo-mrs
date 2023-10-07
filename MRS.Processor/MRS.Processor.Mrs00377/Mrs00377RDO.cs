using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00377
{
    public class Mrs00377RDO
    {
        // báo cáo tổng hợp sử dụng thuốc viện phí

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        public long MEDI_STOCK_ID { get;  set;  }
        
        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string SERVICE_CODE { get;  set;  }


        public long SERVICE_ID { get;  set;  }

        public string CONCENTRA { get;  set;  }                   // hàm lượng, nồng độ

        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal BEGIN_AMOUNT_TVIEN { get;  set;  }           // tồn cuối kỳ Toàn viện
        public decimal IMP_AMOUNT_TVIEN { get;  set;  }           // tồn cuối kỳ Toàn viện
        public decimal EXP_AMOUNT_TVIEN { get;  set;  }           // tồn cuối kỳ Toàn viện
        public decimal END_AMOUNT_TVIEN { get;  set;  }           // tồn cuối kỳ Toàn viện

        public decimal BEGIN_AMOUNT_TONG { get;  set;  }         // tồn đầu kỳ kho tổng
        public decimal IMP_AMOUNT_TONG { get;  set;  }           // nhập trong kỳ kho tổng
        public decimal EXP_AMOUNT_TONG { get;  set;  }           // xuất trong kỳ kho tổng
        public decimal END_AMOUNT_TONG { get;  set;  }           // tồn cuối kỳ kho tổng

        public decimal BEGIN_AMOUNT_NOI_TRU { get;  set;  }      // tồn đầu kỳ kho nội trú
        public decimal IMP_AMOUNT_NOI_TRU { get;  set;  }        // tổng nhập trong kỳ về kho nội trú
        public decimal IMP_AMOUNT_NOI_TRU_MOBA { get;  set;  }   // nhập thu hồi về kho nội trú
        public decimal IMP_AMOUNT_NOI_TRU_KT { get;  set;  }     // nhập từ kho tổng -> kho nội trú
        public decimal EXP_AMOUNT_NOI_TRU { get;  set;  }        // xuất trong kỳ kho nội trú
        public decimal END_AMOUNT_NOI_TRU { get;  set;  }        // tồn cuối kỳ kho nội trú

        public decimal BEGIN_AMOUNT_NGOAI_TRU { get;  set;  }    // tồn đầu kỳ kho ngoại trú
        public decimal IMP_AMOUNT_NGOAI_TRU { get;  set;  }      // tổng nhập trong kỳ về kho ngoại trú
        public decimal IMP_AMOUNT_NGOAI_TRU_MOBA { get;  set;  } // nhập thu hồi về kho ngoại trú
        public decimal IMP_AMOUNT_NGOAI_TRU_KT { get;  set;  }   // nhập từ kho tổng -> kho ngoại trú
        public decimal EXP_AMOUNT_NGOAI_TRU { get;  set;  }      // xuất trong kỳ kho ngoại trú
        public decimal END_AMOUNT_NGOAI_TRU { get;  set;  }   
        // tồn cuối kỳ kho ngoại trú
        public decimal BEGIN_AMOUNT_CONG_DONG { get;  set;  }    // tồn đầu kỳ kho cộng đồng
        public decimal IMP_AMOUNT_CONG_DONG { get;  set;  }      // tổng nhập trong kỳ về kho cộng đồng
        public decimal IMP_AMOUNT_CONG_DONG_MOBA { get;  set;  } // nhập thu hồi về kho cộng đồng
        public decimal IMP_AMOUNT_CONG_DONG_KT { get;  set;  }   // nhập từ kho tổng -> kho cộng đồng
        public decimal EXP_AMOUNT_CONG_DONG { get;  set;  }      // xuất trong kỳ kho cộng đồng
        public decimal END_AMOUNT_CONG_DONG { get;  set;  }      // tồn cuối kỳ kho cộng đồng

        public Mrs00377RDO() { }
    }
}
