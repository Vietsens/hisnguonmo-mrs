using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00153
{
    public class Mrs00153Filter
    {
        public long? DATE_FROM { get; set; }

        public long? DATE_TO { get; set; }

        public long? IMP_MEST_STT_ID { get; set; }//trạng thái nhập kho

        public long? MEDI_STOCK_ID { get; set; }//kho nhập (ghi rõ UI kho nhập thuốc)

        public List<long> IMP_MEST_STT_IDs { get; set; }//trạng thái nhập kho

        public List<long> MEDI_STOCK_IDs { get; set; }//kho nhập (ghi rõ UI kho nhập thuốc)

        public List<long> REQ_DEPARTMENT_IDs { get; set; }

        public List<long> EXACT_BED_ROOM_IDs { get; set; }

        public List<long> REQ_ROOM_IDs { get; set; }

        public long? MEDI_STOCK_CABINET_ID { get; set; }//tủ trực xuất
        public List<long> MEDI_STOCK_CABINET_IDs { get; set; }//tủ trực xuất

        public List<long> IMP_MEST_TYPE_IDs { get; set; }//loại phiếu trả
    }
}
