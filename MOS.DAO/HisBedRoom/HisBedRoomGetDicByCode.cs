using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedRoom
{
    partial class HisBedRoomGet : EntityBase
    {
        public Dictionary<string, HIS_BED_ROOM> GetDicByCode(HisBedRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_BED_ROOM> dic = new Dictionary<string, HIS_BED_ROOM>();
            try
            {
                List<HIS_BED_ROOM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.BED_ROOM_CODE))
                        {
                            dic.Add(item.BED_ROOM_CODE, item);
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
