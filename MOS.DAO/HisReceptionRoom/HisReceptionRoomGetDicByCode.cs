using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisReceptionRoom
{
    partial class HisReceptionRoomGet : EntityBase
    {
        public Dictionary<string, HIS_RECEPTION_ROOM> GetDicByCode(HisReceptionRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_RECEPTION_ROOM> dic = new Dictionary<string, HIS_RECEPTION_ROOM>();
            try
            {
                List<HIS_RECEPTION_ROOM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.RECEPTION_ROOM_CODE))
                        {
                            dic.Add(item.RECEPTION_ROOM_CODE, item);
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
