using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisOtherPaySource
{
    partial class HisOtherPaySourceGet : EntityBase
    {
        public Dictionary<string, HIS_OTHER_PAY_SOURCE> GetDicByCode(HisOtherPaySourceSO search, CommonParam param)
        {
            Dictionary<string, HIS_OTHER_PAY_SOURCE> dic = new Dictionary<string, HIS_OTHER_PAY_SOURCE>();
            try
            {
                List<HIS_OTHER_PAY_SOURCE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.OTHER_PAY_SOURCE_CODE))
                        {
                            dic.Add(item.OTHER_PAY_SOURCE_CODE, item);
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
