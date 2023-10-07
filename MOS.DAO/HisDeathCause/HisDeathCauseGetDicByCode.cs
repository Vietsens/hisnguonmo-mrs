using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDeathCause
{
    partial class HisDeathCauseGet : EntityBase
    {
        public Dictionary<string, HIS_DEATH_CAUSE> GetDicByCode(HisDeathCauseSO search, CommonParam param)
        {
            Dictionary<string, HIS_DEATH_CAUSE> dic = new Dictionary<string, HIS_DEATH_CAUSE>();
            try
            {
                List<HIS_DEATH_CAUSE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.DEATH_CAUSE_CODE))
                        {
                            dic.Add(item.DEATH_CAUSE_CODE, item);
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
