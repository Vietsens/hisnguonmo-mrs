using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00076
{
    /// <summary>
    /// Báo cáo chi tiết thẻ kho thuốc theo ngày
    /// 
    /// </summary>
    public class Mrs00076Filter
    {
        //Bắt buộc phải truyền vào kho và thuốc
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public long MATERIAL_TYPE_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public Mrs00076Filter()
            : base()
        {

        }
    }
}
