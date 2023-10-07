using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.MRS00362
{
    public class Mrs00362RDO
    {
        public long TYPE { get; set; }
        public long SERVICE_ID { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public long? CREAT_TIME { get; set; }
        public long? EXP_TIME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }
        public long MEDICINE_GROUP_ID { get; set; }
        public string CONCENTRA { get; set; }
        public string MANUFACTURER { get; set; }
        public string NATIONAL_NAME { get; set; }
        //thong tin lo
        public long MEMA_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string EXPIRED_DATE { get; set; }
        public string EXP_MEST_REASON_NAME { set; get; }
        public Dictionary<string, decimal> DIC_REASON { get; set; }

       

        public Mrs00362RDO()
        {
            DIC_REASON = new Dictionary<string, decimal>();
        }
    }
}
