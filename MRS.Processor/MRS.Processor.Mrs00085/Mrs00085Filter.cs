using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00085
{
    /// <summary>
    /// Báo cáo chi tiết thẻ kho thuốc theo ngày
    /// 
    /// </summary>
    public class Mrs00085Filter
    {
        //Bắt buộc phải truyền vào kho và thuốc
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public long? MATERIAL_TYPE_ID { get; set; }

        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public List<long> MATERIAL_IDs { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool? IS_NOT_SHOW_ALL { get; set; }

        public Mrs00085Filter()
            : base()
        {

        }
    }
}
