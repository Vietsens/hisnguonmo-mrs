using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHelmet
{
    partial class HisAccidentHelmetGet : EntityBase
    {
        public Dictionary<string, HIS_ACCIDENT_HELMET> GetDicByCode(HisAccidentHelmetSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_HELMET> dic = new Dictionary<string, HIS_ACCIDENT_HELMET>();
            try
            {
                List<HIS_ACCIDENT_HELMET> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ACCIDENT_HELMET_CODE))
                        {
                            dic.Add(item.ACCIDENT_HELMET_CODE, item);
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
