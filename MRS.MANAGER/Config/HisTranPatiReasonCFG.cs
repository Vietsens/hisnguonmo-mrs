using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisTranPatiReason;
using Inventec.Core;

namespace MRS.MANAGER.Config
{
    public class HisTranPatiReasonCFG
    {
        private const string HIS_TRAN_PATI_REASON__01 = "MRS.HIS_TRAN_PATI_REASON.TRAN_PATI_REASON_CODE__01";//Chuyển người bệnh đi các tuyến khi đủ điều kiện
        private const string HIS_TRAN_PATI_REASON__02 = "MRS.HIS_TRAN_PATI_REASON.TRAN_PATI_REASON_CODE__02";//Chuyển theo yêu cầu của người bệnh/đại diện hợp pháp của người bệnh

        private static long hisTranPatiReason02;
        public static long HIS_TRAN_PATI_REASON___02
        {
            get
            {
                if (hisTranPatiReason02 == 0)
                {
                    hisTranPatiReason02 = GetId(HIS_TRAN_PATI_REASON__02);
                }
                return hisTranPatiReason02;
            }
            set
            {
                hisTranPatiReason02 = value;
            }
        }

        private static long hisTranPatiReason01;
        public static long HIS_TRAN_PATI_REASON___01
        {
            get
            {
                if (hisTranPatiReason01 == 0)
                {
                    hisTranPatiReason01 = GetId(HIS_TRAN_PATI_REASON__01);
                }
                return hisTranPatiReason01;
            }
            set
            {
                hisTranPatiReason01 = value;
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

                var filter = new HisTranPatiReasonFilterQuery();
                var data = new HisTranPatiReasonManager(new CommonParam()).Get(filter).FirstOrDefault(o => o.TRAN_PATI_REASON_CODE == value);
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

        public static void Refresh()
        {
            try
            {
                hisTranPatiReason02 = 0;
                hisTranPatiReason01 = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
