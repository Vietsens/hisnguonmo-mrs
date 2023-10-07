using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisSampleRoom
{
    partial class HisSampleRoomGet : EntityBase
    {
        public Dictionary<string, HIS_SAMPLE_ROOM> GetDicByCode(HisSampleRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_SAMPLE_ROOM> dic = new Dictionary<string, HIS_SAMPLE_ROOM>();
            try
            {
                List<HIS_SAMPLE_ROOM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.SAMPLE_ROOM_CODE))
                        {
                            dic.Add(item.SAMPLE_ROOM_CODE, item);
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
