using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisRepayReason;

namespace MRS.MANAGER.Config
{
    public class HisRepayReasonCFG
    {
        /// <summary>
        ///REPAY_REASON_CODE__01. hoàn lại tạm ứng nội trú
        ///REPAY_REASON_CODE__02. hoàn tiền ngoại trú - cập nhật thẻ
        ///REPAY_REASON_CODE__03. hoàn ứng nội trú ra viện
        /// </summary>
        private const string REPAY_REASON_CODE__01 = "MRS.HIS_RS.HIS_REPAY_REASON.REPAY_REASON_CODE__01";
        private const string REPAY_REASON_CODE__02 = "MRS.HIS_RS.HIS_REPAY_REASON.REPAY_REASON_CODE__02";
        private const string REPAY_REASON_CODE__03 = "MRS.HIS_RS.HIS_REPAY_REASON.REPAY_REASON_CODE__03";
        private const string REPAY_REASON_CODE__04 = "MRS.HIS_RS.HIS_REPAY_REASON.REPAY_REASON_CODE__04";

        private static long RepayReason01;
        /// <summary>
        /// hoàn lại tạm ứng nội trú
        /// </summary>
        public static long get_REPAY_REASON_CODE__01
        {
            get
            {
                if (RepayReason01 == 0)
                {
                    RepayReason01 = GetId(REPAY_REASON_CODE__01);
                }
                return RepayReason01;
            }
        }

        private static long RepayReason02;
        /// <summary>
        /// hoàn tiền ngoại trú - cập nhật thẻ
        /// </summary>
        public static long get_REPAY_REASON_CODE__02
        {
            get
            {
                if (RepayReason02 == 0)
                {
                    RepayReason02 = GetId(REPAY_REASON_CODE__02);
                }
                return RepayReason02;
            }
        }

        private static long RepayReason03;
        /// <summary>
        /// hoàn ứng nội trú ra viện
        /// </summary>
        public static long get_REPAY_REASON_CODE__03
        {
            get
            {
                if (RepayReason03 == 0)
                {
                    RepayReason03 = GetId(REPAY_REASON_CODE__03);
                }
                return RepayReason03;
            }
        }

        private static long RepayReason04;
        public static long get_REPAY_REASON_CODE__04
        {
            get
            {
                if (RepayReason04 == 0)
                {
                    RepayReason04 = GetId(REPAY_REASON_CODE__04);
                }
                return RepayReason04;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_REPAY_REASON> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_REPAY_REASON> result = null;
            try
            {
                HisRepayReasonFilterQuery filter = new HisRepayReasonFilterQuery();
                result = new HisRepayReasonManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                //filter.SERVICE_REQ_TYPE_CODE = code;
                var data = GetAll().FirstOrDefault(o => o.REPAY_REASON_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
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
                RepayReason01 = 0;
                RepayReason02 = 0;
                RepayReason03 = 0;
                RepayReason04 = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
