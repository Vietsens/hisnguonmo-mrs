using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAnticipateMaty
{
    partial class HisAnticipateMatyGet : EntityBase
    {
        public V_HIS_ANTICIPATE_MATY GetViewById(long id, HisAnticipateMatySO search)
        {
            V_HIS_ANTICIPATE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_ANTICIPATE_MATY.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisAnticipateMatyExpression != null && search.listVHisAnticipateMatyExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisAnticipateMatyExpression)
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
