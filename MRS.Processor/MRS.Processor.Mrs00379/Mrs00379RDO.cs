using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00379
{
    public class Mrs00379RDO:IMP_EXP_MEST_TYPE
    {
        // báo cáo xuất nhập tồn theo kho

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }

        public long PARENT_ID { get;  set;  }
        public string PARENT_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }

        public string CONCENTRA { get;  set;  }               // nồng độ/hàm lượng

        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal IMP_PRICE { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal? END_AMOUNT { get;  set;  }

        public decimal IMP_MEST_CHMS_REPO { get;  set;  }     // nhập chuyển kho giữa các kho báo cáo
        public decimal IMP_MEST_MOBA_BHYT { get;  set;  }     // nhập trả về kho bhyt
        public decimal IMP_MEST_MOBA_CLIN { get;  set;  }     // nhập trả về kho nội trú
        public decimal IMP_MEST_MOBA_CABI { get;  set;  }     // nhập trả về kho tủ trực
        public decimal IMP_MEST_CHMS_BUSI { get;  set;  }     // nhập chuyển kho từ kho kinh doanh
        

        public decimal EXP_MEST_PRES_BHYT { get;  set;  }     // xuất đơn thuốc kho bhyt
        public decimal EXP_MEST_PRES_CLIN { get;  set;  }     // xuất đơn thuốc kho nội trú
        public decimal EXP_MEST_PRES_CABI { get;  set;  }     // xuất đơn thuốc tủ trực
        public decimal EXP_MEST_CHMS_BUSI { get;  set;  }     // xuất chuyển kho cho kho kinh doanh
        public decimal EXP_MEST_CHMS_REPO { get;  set;  }     // xuất chuyển kho giữa các kho báo cáo

        public decimal? EXP_MEST_REASON_0 { get;  set;  }
        public decimal? EXP_MEST_REASON_1 { get;  set;  }
        public decimal? EXP_MEST_REASON_2 { get;  set;  }
        public decimal? EXP_MEST_REASON_3 { get;  set;  }
        public decimal? EXP_MEST_REASON_4 { get;  set;  }
        public decimal? EXP_MEST_REASON_5 { get;  set;  }
        public decimal? EXP_MEST_REASON_6 { get;  set;  }
        public decimal? EXP_MEST_REASON_7 { get;  set;  }
        public decimal? EXP_MEST_REASON_8 { get;  set;  }
        public decimal? EXP_MEST_REASON_9 { get;  set;  }

        public Mrs00379RDO() { }
    }

    public class EXP_MEST_REASON
    {
        public long EXP_MEST_REASON_ID { get;  set;  }
        public string EXP_MEST_REASON_NAME { get;  set;  }
        public string EXP_MEST_REASON_TAG { get;  set;  }
    }
}
