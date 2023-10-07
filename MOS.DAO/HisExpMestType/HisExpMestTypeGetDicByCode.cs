using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExpMestType
{
    partial class HisExpMestTypeGet : EntityBase
    {
        public Dictionary<string, HIS_EXP_MEST_TYPE> GetDicByCode(HisExpMestTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXP_MEST_TYPE> dic = new Dictionary<string, HIS_EXP_MEST_TYPE>();
            try
            {
                List<HIS_EXP_MEST_TYPE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.EXP_MEST_TYPE_CODE))
                        {
                            dic.Add(item.EXP_MEST_TYPE_CODE, item);
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
