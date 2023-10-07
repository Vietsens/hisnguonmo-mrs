using MOS.EFMODEL.DataModels;
using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00713
{

    public class Mrs00713Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public string SERVICE_CODE__LZMMs { get; set; }// 6. Tổng số lượng thủ thuật Laser mống mắt (Mã DV: TT13_PT23)
        public string SERVICE_CODE__LZQDs { get; set; }//7. Tổng số lượng thủ thuật Laser Quang đông (Mã DV: TT13_TT81)
        public string SERVICE_CODE__TNNs { get; set; }//8. Tổng số lượng thủ thuật Tiêm nội nhãn (Mã DV: TT13_PT88)
        public string SERVICE_CODE__LZBSs { get; set; }//10. Tổng số lượng thủ thuật Laser bao sau (Mã DV: TT13_PT79)


        public bool? IS_NGOAITRU { get; set; }
        public bool? IS_NOITRU { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

    }
}
