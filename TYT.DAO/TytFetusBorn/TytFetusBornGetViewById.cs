using TYT.DAO.Base;
using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TYT.DAO.TytFetusBorn
{
    partial class TytFetusBornGet : EntityBase
    {
        public V_TYT_FETUS_BORN GetViewById(long id, TytFetusBornSO search)
        {
            V_TYT_FETUS_BORN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_TYT_FETUS_BORN.AsQueryable().Where(p => p.ID == id);
                        if (search.listVTytFetusBornExpression != null && search.listVTytFetusBornExpression.Count > 0)
                        {
                            foreach (var item in search.listVTytFetusBornExpression)
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
