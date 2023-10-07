using HTC.DAO.Base;
using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HTC.DAO.HtcPeriodDepartment
{
    partial class HtcPeriodDepartmentGet : EntityBase
    {
        public V_HTC_PERIOD_DEPARTMENT GetViewById(long id, HtcPeriodDepartmentSO search)
        {
            V_HTC_PERIOD_DEPARTMENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_HTC_PERIOD_DEPARTMENT.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHtcPeriodDepartmentExpression != null && search.listVHtcPeriodDepartmentExpression.Count > 0)
                        {
                            foreach (var item in search.listVHtcPeriodDepartmentExpression)
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
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => id), id) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
