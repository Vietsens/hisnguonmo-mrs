using TYT.DAO.Base;
using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TYT.DAO.TytUninfectIcdGroup
{
    partial class TytUninfectIcdGroupGet : EntityBase
    {
        public Dictionary<string, TYT_UNINFECT_ICD_GROUP> GetDicByCode(TytUninfectIcdGroupSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT_ICD_GROUP> dic = new Dictionary<string, TYT_UNINFECT_ICD_GROUP>();
            try
            {
                List<TYT_UNINFECT_ICD_GROUP> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.UNINFECT_ICD_GROUP_CODE))
                        {
                            dic.Add(item.UNINFECT_ICD_GROUP_CODE, item);
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
