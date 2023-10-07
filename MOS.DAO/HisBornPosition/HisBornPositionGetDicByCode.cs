using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisBornPosition
{
    partial class HisBornPositionGet : EntityBase
    {
        public Dictionary<string, HIS_BORN_POSITION> GetDicByCode(HisBornPositionSO search, CommonParam param)
        {
            Dictionary<string, HIS_BORN_POSITION> dic = new Dictionary<string, HIS_BORN_POSITION>();
            try
            {
                List<HIS_BORN_POSITION> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.BORN_POSITION_CODE))
                        {
                            dic.Add(item.BORN_POSITION_CODE, item);
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
