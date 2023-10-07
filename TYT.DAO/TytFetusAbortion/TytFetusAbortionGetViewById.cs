using TYT.DAO.Base;
using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TYT.DAO.TytFetusAbortion
{
    partial class TytFetusAbortionGet : EntityBase
    {
        public V_TYT_FETUS_ABORTION GetViewById(long id, TytFetusAbortionSO search)
        {
            V_TYT_FETUS_ABORTION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_TYT_FETUS_ABORTION.AsQueryable().Where(p => p.ID == id);
                        if (search.listVTytFetusAbortionExpression != null && search.listVTytFetusAbortionExpression.Count > 0)
                        {
                            foreach (var item in search.listVTytFetusAbortionExpression)
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
