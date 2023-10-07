using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAnticipateMety
{
    partial class HisAnticipateMetyGet : EntityBase
    {
        public V_HIS_ANTICIPATE_METY GetViewById(long id, HisAnticipateMetySO search)
        {
            V_HIS_ANTICIPATE_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_ANTICIPATE_METY.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisAnticipateMetyExpression != null && search.listVHisAnticipateMetyExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisAnticipateMetyExpression)
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
