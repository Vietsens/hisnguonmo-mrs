using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisMemaGroup
{
    partial class HisMemaGroupGet : EntityBase
    {
        public Dictionary<string, HIS_MEMA_GROUP> GetDicByCode(HisMemaGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEMA_GROUP> dic = new Dictionary<string, HIS_MEMA_GROUP>();
            try
            {
                List<HIS_MEMA_GROUP> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.MEMA_GROUP_CODE))
                        {
                            dic.Add(item.MEMA_GROUP_CODE, item);
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
