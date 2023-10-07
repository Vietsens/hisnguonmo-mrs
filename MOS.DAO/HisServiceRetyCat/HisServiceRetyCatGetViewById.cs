using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisServiceRetyCat
{
    partial class HisServiceRetyCatGet : EntityBase
    {
        public V_HIS_SERVICE_RETY_CAT GetViewById(long id, HisServiceRetyCatSO search)
        {
            V_HIS_SERVICE_RETY_CAT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_SERVICE_RETY_CAT.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisServiceRetyCatExpression != null && search.listVHisServiceRetyCatExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisServiceRetyCatExpression)
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
