using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00728
{
    class Mrs00728RDO
    {
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MANUFRACTURER_CODE { get; set; }
        public string MANUFRACTURER_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }
        public long EXP_TIME { get; set; }
        public string EXP_TIME_STR { get; set; }
        public decimal AMOUNT { get; set; }

        public decimal? PRICE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
    }
}
