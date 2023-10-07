using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00557
{
    public class Mrs00557RDO : HIS_SERE_SERV
    {
        public HIS_SERVICE_REQ HIS_SERVICE_REQ { get; set; }
        public string TYPE { get; set; }

        public string GR_CODE { get; set; }

        public string GR_NAME { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public long TREATMENT_TYPE_ID { get; set; }

        public int COUNT_TREATMENT_BED_ROOM_IN { get; set; }

        public int COUNT_TREATMENT_BED_ROOM_OUT { get; set; }

        public int COUNT_TREATMENT_IN { get; set; }

        public int COUNT_TREATMENT_OUT { get; set; }

        public Dictionary<string, int> DICT_COUNT_IN { get; set; }

        public Dictionary<string, int> DICT_COUNT_OUT { get; set; }

        public Dictionary<string, int> DICT_COUNT_TOTAL { get; set; }

        public Dictionary<string, decimal> DICT_AMOUNT_IN { get; set; }

        public Dictionary<string, decimal> DICT_AMOUNT_OUT { get; set; }

        public Dictionary<string, decimal> DICT_AMOUNT_TOTAL { get; set; }

        public Dictionary<string, int> DICG_COUNT_IN { get; set; }

        public Dictionary<string, int> DICG_COUNT_OUT { get; set; }

        public Dictionary<string, int> DICG_COUNT_TOTAL { get; set; }

        public Dictionary<string, decimal> DICG_AMOUNT_IN { get; set; }

        public Dictionary<string, decimal> DICG_AMOUNT_OUT { get; set; }

        public Dictionary<string, decimal> DICG_AMOUNT_TOTAL { get; set; }
    
        public Dictionary<string, decimal> DICG_MONEY_TOTAL { get; set; }
    
        public Mrs00557RDO()
        {
            DICT_COUNT_IN = new Dictionary<string, int>();
            DICT_COUNT_OUT = new Dictionary<string, int>();
            DICT_COUNT_TOTAL = new Dictionary<string, int>();

            DICT_AMOUNT_IN = new Dictionary<string, decimal>();
            DICT_AMOUNT_OUT = new Dictionary<string, decimal>();
            DICT_AMOUNT_TOTAL = new Dictionary<string, decimal>();

            DICG_COUNT_IN = new Dictionary<string, int>();
            DICG_COUNT_OUT = new Dictionary<string, int>();
            DICG_COUNT_TOTAL = new Dictionary<string, int>();

            DICG_AMOUNT_IN = new Dictionary<string, decimal>();
            DICG_AMOUNT_OUT = new Dictionary<string, decimal>();
            DICG_AMOUNT_OUT = new Dictionary<string, decimal>();
            DICG_MONEY_TOTAL = new Dictionary<string, decimal>();
        }



        }
}
