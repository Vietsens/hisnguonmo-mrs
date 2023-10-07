using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAwareness
{
    partial class HisAwarenessGet : EntityBase
    {
        public Dictionary<string, HIS_AWARENESS> GetDicByCode(HisAwarenessSO search, CommonParam param)
        {
            Dictionary<string, HIS_AWARENESS> dic = new Dictionary<string, HIS_AWARENESS>();
            try
            {
                List<HIS_AWARENESS> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.AWARENESS_CODE))
                        {
                            dic.Add(item.AWARENESS_CODE, item);
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
