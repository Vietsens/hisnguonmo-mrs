using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHurtType
{
    partial class HisAccidentHurtTypeGet : EntityBase
    {
        public Dictionary<string, HIS_ACCIDENT_HURT_TYPE> GetDicByCode(HisAccidentHurtTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_HURT_TYPE> dic = new Dictionary<string, HIS_ACCIDENT_HURT_TYPE>();
            try
            {
                List<HIS_ACCIDENT_HURT_TYPE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ACCIDENT_HURT_TYPE_CODE))
                        {
                            dic.Add(item.ACCIDENT_HURT_TYPE_CODE, item);
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
