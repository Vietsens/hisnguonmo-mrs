using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00306
{
    public class Mrs00306RDO
    {
        public string  SERVICE_NAME { get;  set;  } // Tên thuốc vật tư
        public string  SERVICE_UNIT_NAME { get;  set;  } // Đơn vị tính
        public decimal PRICE { get;  set;  } // Đơn giá
        public decimal AMOUNT { get;  set;  } //Số lượng
        public decimal TOTAL_PRICE { get;  set;  } //Thành Tiền
        public string NATIONAL_NAME { get;  set;  } //Nước sản xuất
       

        //Thêm các trường lấy dữ liệu
        public string IMP_TIME { get;  set;  } //thời gian thu hôi thu hồi
        public string CLIENT_NAME { get;  set;  } // Tên người trả
        public string IMP_MEST_CODE { get;  set;  } // Mã phiếu thu hồi
        public string EXP_MEST_CODE { get;  set;  } // Mã phiếu xuất bán
        public string EXP_TIME { get;  set;  } // Ngày bán


        public decimal IMP_PRICE { set; get; }
        public decimal TOTAL_IMP_PRICE { set; get; }

        public decimal COUNT_IMP_EXP_MEST { set; get; }

        public string CONCENTRA { get; set; }

        public decimal IMP_PRICE_B4_VAT { get; set; }

        public decimal? VAT { get; set; }

        public decimal EXP_VAT { get; set; }

        public decimal IMP_VAT { get; set; }

        public decimal IMP_PRICE_ORIGIN { get; set; }
    }
}
