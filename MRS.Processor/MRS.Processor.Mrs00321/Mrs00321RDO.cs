using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00321
{
    public class Mrs00321RDO
    {
        public string REQ_USERNAME { get; set; }    // Tên người tạo xuất bán
        public string EXE_USERNAME { get; set; }    // Tên người thực xuất
        public decimal BILL_AMOUNT { get; set; }        // Số lượng hóa đơn
        public decimal TOTAL_PRICE { get; set; }        // tổng số tiền bán + VAT
        public decimal TOTAL_IMP_PRICE { get; set; }    // tổng tiền theo giá nhập + VAT

        public decimal AMOUNT { get; set; }             // Số lượng thuốc, vật tư
        public decimal VAT_RATIO { get; set; }          // Thuế
        public decimal PRICE { get; set; }              // Đơn giá bán
        public decimal IMP_PRICE { get; set; }          // Đơn giá nhập

        public long EXP_MEST_ID { get; set; }           // id phiếu xuất

        public string CASHIER_USERNAME { get; set; }
    }
}
