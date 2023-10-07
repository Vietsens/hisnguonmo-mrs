using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisRepayReason
{
    partial class HisRepayReasonGet : EntityBase
    {
        public Dictionary<string, HIS_REPAY_REASON> GetDicByCode(HisRepayReasonSO search, CommonParam param)
        {
            Dictionary<string, HIS_REPAY_REASON> dic = new Dictionary<string, HIS_REPAY_REASON>();
            try
            {
                List<HIS_REPAY_REASON> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.REPAY_REASON_CODE))
                        {
                            dic.Add(item.REPAY_REASON_CODE, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param), LogType.Error);
                LogSystem.Error(ex);
                dic.Clear();
            }
            return dic;
        }
    }
}
