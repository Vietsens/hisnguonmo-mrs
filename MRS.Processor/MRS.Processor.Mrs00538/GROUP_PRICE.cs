using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00538
{
    public class GROUP_PRICE
    {
        public Decimal COUNT_OUT { get; set; }//Số BN ra viện
        public Decimal SUM_TOTAL_PRICE { get; set; }// Tổng tiền
        public Decimal SUM_TOTAL_HEIN_PRICE { get; set; }// Tổng tiền Bảo Hiểm thanh toán
        public Decimal INFO_AVG { get; set; }// Chi phí bình quân

    }
}
