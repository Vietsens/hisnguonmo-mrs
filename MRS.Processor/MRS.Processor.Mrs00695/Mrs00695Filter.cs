using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00695
{
    public class Mrs00695Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? MEDI_STOCK_CABINET_ID { get; set; }//tủ trực
        public long? MEST_ROOM_STOCK_ID { get; set; }//medi_stock_id kho xuất tương ứng
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }

        /// <summary>
        /// Ky bat dau tinh, ngay dau tien cua bao cao se la ngay dau tien cua ky.
        /// Trong truong hop null, se duoc hieu la ky hien tai (chua chot).
        /// </summary>
        public long? MEDI_STOCK_PERIOD_ID { get; set; }

        public List<long> MEDI_STOCK_CABINET_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public bool? IS_GROUP { get; set; }
    }
}
