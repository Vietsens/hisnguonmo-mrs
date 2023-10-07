using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00675
{
    class Mrs00675RDO
    {
        public long ROW_POS { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public Dictionary<string, long> DIC_ROOM_COUNT { get; set; }// key: mã phòng chỉ định, Value: số lượt chỉ định
    }
}
