using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisCashierRoom
{
    partial class HisCashierRoomGet : EntityBase
    {
        public Dictionary<string, HIS_CASHIER_ROOM> GetDicByCode(HisCashierRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_CASHIER_ROOM> dic = new Dictionary<string, HIS_CASHIER_ROOM>();
            try
            {
                List<HIS_CASHIER_ROOM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.CASHIER_ROOM_CODE))
                        {
                            dic.Add(item.CASHIER_ROOM_CODE, item);
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
