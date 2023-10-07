using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00133
{
    class Mrs00133RDO : IMP_EXP_MEST_TYPE
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public string MEDI_STOCK_NAME { get;  set;  }

        public int SERVICE_TYPE_ID { get;  set;  }

        public long MEDI_MATE_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }

        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }
        public decimal IMP_PRICE { get;  set;  }
        public decimal IMP_VAT { get;  set;  }

        public string CONCENTRA { get; set; }//Hàm lượng
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get; set; }//Nước sản xuất

        public long? NUM_ORDER { get;  set;  }
        public decimal? PRICE { get; set; }
        public decimal EXP_PRICE { get; set; }
        public decimal EXP_VAT { get; set; }
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string PACKAGE_NUMBER { get; set; }//Số lô
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu
        public string BID_NUMBER { get; set; }//Quyết đinh thầu
        public string BID_YEAR { get; set; }//năm thầu
        public string BID_PACKAGE_CODE { get; set; }//gói thầu
        public string HEIN_SERVICE_BHYT_CODE { get; set; }//Mã BHYT
        public string HEIN_SERVICE_BHYT_NAME { get; set; }//Tên BHYT
        public string IMP_SOURCE_NAME { get; set; }//Nguồn nhập
        public string ATC_CODES { get; set; }//Mã ATC
        public string NATIONAL_NAME { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_LINE_CODE { get; set; }
        public string MEDICINE_LINE_NAME { get; set; }
        public long? MEDICINE_GROUP_ID { get; set; }
        public long PARENT_MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }
    }
}
