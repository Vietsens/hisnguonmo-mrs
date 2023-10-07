using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisBhytBlacklist
{
    partial class HisBhytBlacklistGet : EntityBase
    {
        public Dictionary<string, HIS_BHYT_BLACKLIST> GetDicByCode(HisBhytBlacklistSO search, CommonParam param)
        {
            Dictionary<string, HIS_BHYT_BLACKLIST> dic = new Dictionary<string, HIS_BHYT_BLACKLIST>();
            try
            {
                List<HIS_BHYT_BLACKLIST> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.HEIN_CARD_NUMBER))
                        {
                            dic.Add(item.HEIN_CARD_NUMBER, item);
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
