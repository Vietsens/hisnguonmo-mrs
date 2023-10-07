using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPtttPriority
{
    partial class HisPtttPriorityGet : EntityBase
    {
        public HIS_PTTT_PRIORITY GetByCode(string code, HisPtttPrioritySO search)
        {
            HIS_PTTT_PRIORITY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_PTTT_PRIORITY.AsQueryable().Where(p => p.PTTT_PRIORITY_CODE == code);
                        if (search.listHisPtttPriorityExpression != null && search.listHisPtttPriorityExpression.Count > 0)
                        {
                            foreach (var item in search.listHisPtttPriorityExpression)
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
