using MOS.EFMODEL.DataModels;
using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00712
{

    public class Mrs00712Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public string SERVICE_CODE__LZMMs { get; set; }// 6. Tổng số lượng thủ thuật Laser mống mắt (Mã DV: TT13_PT23)
        public string SERVICE_CODE__LZQDs { get; set; }//7. Tổng số lượng thủ thuật Laser Quang đông (Mã DV: TT13_TT81)
        public string SERVICE_CODE__TNNs { get; set; }//8. Tổng số lượng thủ thuật Tiêm nội nhãn (Mã DV: TT13_PT88)
        public string SERVICE_CODE__LZBSs { get; set; }//10. Tổng số lượng thủ thuật Laser bao sau (Mã DV: TT13_PT79)

    }
}
