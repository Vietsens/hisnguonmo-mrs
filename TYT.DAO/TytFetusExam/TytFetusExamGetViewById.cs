using TYT.DAO.Base;
using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TYT.DAO.TytFetusExam
{
    partial class TytFetusExamGet : EntityBase
    {
        public V_TYT_FETUS_EXAM GetViewById(long id, TytFetusExamSO search)
        {
            V_TYT_FETUS_EXAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_TYT_FETUS_EXAM.AsQueryable().Where(p => p.ID == id);
                        if (search.listVTytFetusExamExpression != null && search.listVTytFetusExamExpression.Count > 0)
                        {
                            foreach (var item in search.listVTytFetusExamExpression)
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
