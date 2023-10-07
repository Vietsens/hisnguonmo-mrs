using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExecuteRoom
{
    partial class HisExecuteRoomGet : EntityBase
    {
        public V_HIS_EXECUTE_ROOM GetViewByCode(string code, HisExecuteRoomSO search)
        {
            V_HIS_EXECUTE_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_EXECUTE_ROOM.AsQueryable().Where(p => p.EXECUTE_ROOM_CODE == code);
                        if (search.listVHisExecuteRoomExpression != null && search.listVHisExecuteRoomExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisExecuteRoomExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        result = query.SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
