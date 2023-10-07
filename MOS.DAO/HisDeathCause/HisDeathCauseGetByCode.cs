using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDeathCause
{
    partial class HisDeathCauseGet : EntityBase
    {
        public HIS_DEATH_CAUSE GetByCode(string code, HisDeathCauseSO search)
        {
            HIS_DEATH_CAUSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_DEATH_CAUSE.AsQueryable().Where(p => p.DEATH_CAUSE_CODE == code);
                        if (search.listHisDeathCauseExpression != null && search.listHisDeathCauseExpression.Count > 0)
                        {
                            foreach (var item in search.listHisDeathCauseExpression)
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
