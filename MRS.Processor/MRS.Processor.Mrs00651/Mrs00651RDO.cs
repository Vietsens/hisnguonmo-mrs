using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00651
{
    class Mrs00651RDO : V_HIS_TRANSACTION
    {
        public string TRANSACTION_DAY_STR { get; set; }
        public string TRANSACTION_HOUR_STR { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }

        public Mrs00651RDO() { }

        public Mrs00651RDO(V_HIS_TRANSACTION data)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00651RDO>(this, data);
                this.TRANSACTION_DAY_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(data.TRANSACTION_TIME);
                this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME);
                this.TRANSACTION_HOUR_STR = ProcessTimeNumberToHourString(data.TRANSACTION_TIME);
            }
        }

        private string ProcessTimeNumberToHourString(long p)
        {
            string result = "";
            try
            {
                if (p > 0)
                {
                    var hour = p.ToString().Substring(8);
                    result = string.Format("{0}:{1}:{2}", hour.Substring(0, 2), hour.Substring(2, 2), hour.Substring(4));
                }
            }
            catch (Exception ex)
            {
                result = p.ToString();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public string DEPARTMENT_NAME { get; set; }

        public string CHECK_EXAM_ROOM { get; set; }

        public string CHECK_BED_ROOM { get; set; }

        public string ROOM_CODE { get; set; }

        public string ROOM_NAME { get; set; }
    }
}
