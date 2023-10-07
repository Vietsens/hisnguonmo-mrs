using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisEmotionlessMethod
{
    partial class HisEmotionlessMethodGet : EntityBase
    {
        public Dictionary<string, HIS_EMOTIONLESS_METHOD> GetDicByCode(HisEmotionlessMethodSO search, CommonParam param)
        {
            Dictionary<string, HIS_EMOTIONLESS_METHOD> dic = new Dictionary<string, HIS_EMOTIONLESS_METHOD>();
            try
            {
                List<HIS_EMOTIONLESS_METHOD> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.EMOTIONLESS_METHOD_CODE))
                        {
                            dic.Add(item.EMOTIONLESS_METHOD_CODE, item);
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
