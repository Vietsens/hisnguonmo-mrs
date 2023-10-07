using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisEmergencyWtime
{
    partial class HisEmergencyWtimeGet : EntityBase
    {
        public HIS_EMERGENCY_WTIME GetByCode(string code, HisEmergencyWtimeSO search)
        {
            HIS_EMERGENCY_WTIME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EMERGENCY_WTIME.AsQueryable().Where(p => p.EMERGENCY_WTIME_CODE == code);
                        if (search.listHisEmergencyWtimeExpression != null && search.listHisEmergencyWtimeExpression.Count > 0)
                        {
                            foreach (var item in search.listHisEmergencyWtimeExpression)
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
