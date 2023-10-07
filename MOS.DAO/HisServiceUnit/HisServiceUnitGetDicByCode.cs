using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisServiceUnit
{
    partial class HisServiceUnitGet : EntityBase
    {
        public Dictionary<string, HIS_SERVICE_UNIT> GetDicByCode(HisServiceUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_UNIT> dic = new Dictionary<string, HIS_SERVICE_UNIT>();
            try
            {
                List<HIS_SERVICE_UNIT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.SERVICE_UNIT_CODE))
                        {
                            dic.Add(item.SERVICE_UNIT_CODE, item);
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
