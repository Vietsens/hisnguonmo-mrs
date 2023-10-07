using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestReason;
using MRS.MANAGER.Config;
namespace MRS.Processor.Mrs00531
{
    public class HisExpMestReasonCFG
    {
        private const string HIS_EXP_MEST_REASON__01 = "MRS.HIS_EXP_MEST_REASON.EXP_MEST_REASON_CODE__01";
        private const string HIS_EXP_MEST_REASON__02 = "MRS.HIS_EXP_MEST_REASON.EXP_MEST_REASON_CODE__02";
        private const string HIS_EXP_MEST_REASON__03 = "MRS.HIS_EXP_MEST_REASON.EXP_MEST_REASON_CODE__03";
        private const string HIS_EXP_MEST_REASON__04 = "MRS.HIS_EXP_MEST_REASON.EXP_MEST_REASON_CODE__04";
        private const string HIS_EXP_MEST_REASON__05 = "MRS.HIS_EXP_MEST_REASON.EXP_MEST_REASON_CODE__05";

        private static long hisExpMestReason05;
        public static long HIS_EXP_MEST_REASON___05
        {
            get
            {
                if (hisExpMestReason05 == 0)
                {
                    hisExpMestReason05 = GetId(HIS_EXP_MEST_REASON__05);
                }
                return hisExpMestReason05;
            }
            set
            {
                hisExpMestReason04 = value;
            }
        }

        private static long hisExpMestReason04;
        public static long HIS_EXP_MEST_REASON___04
        {
            get
            {
                if (hisExpMestReason04 == 0)
                {
                    hisExpMestReason04 = GetId(HIS_EXP_MEST_REASON__04);
                }
                return hisExpMestReason04;
            }
            set
            {
                hisExpMestReason04 = value;
            }
        }
        private static long hisExpMestReason03;
        public static long HIS_EXP_MEST_REASON___03
        {
            get
            {
                if (hisExpMestReason03 == 0)
                {
                    hisExpMestReason03 = GetId(HIS_EXP_MEST_REASON__03);
                }
                return hisExpMestReason03;
            }
            set
            {
                hisExpMestReason03 = value;
            }
        }
        private static long hisExpMestReason02;
        public static long HIS_EXP_MEST_REASON___02
        {
            get
            {
                if (hisExpMestReason02 == 0)
                {
                    hisExpMestReason02 = GetId(HIS_EXP_MEST_REASON__02);
                }
                return hisExpMestReason02;
            }
            set
            {
                hisExpMestReason02 = value;
            }
        }

        private static long hisExpMestReason01;
        public static long HIS_EXP_MEST_REASON___01
        {
            get
            {
                if (hisExpMestReason01 == 0)
                {
                    hisExpMestReason01 = GetId(HIS_EXP_MEST_REASON__01);
                }
                return hisExpMestReason01;
            }
            set
            {
                hisExpMestReason01 = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(code);

                var filter = new HisExpMestReasonFilterQuery();
                var data = new HisExpMestReasonManager().Get(filter).FirstOrDefault(o => o.EXP_MEST_REASON_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }
    }
}
