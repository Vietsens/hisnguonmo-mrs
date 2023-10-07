using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAccidentHurt
{
    partial class HisAccidentHurtGet : EntityBase
    {
        public V_HIS_ACCIDENT_HURT GetViewById(long id, HisAccidentHurtSO search)
        {
            V_HIS_ACCIDENT_HURT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_ACCIDENT_HURT.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisAccidentHurtExpression != null && search.listVHisAccidentHurtExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisAccidentHurtExpression)
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
