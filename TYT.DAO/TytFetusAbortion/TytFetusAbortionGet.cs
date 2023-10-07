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
        public List<TYT_FETUS_ABORTION> Get(TytFetusAbortionSO search, CommonParam param)
        {
            List<TYT_FETUS_ABORTION> list = new List<TYT_FETUS_ABORTION>();
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.TYT_FETUS_ABORTION.AsQueryable();
                        if (search.listTytFetusAbortionExpression != null && search.listTytFetusAbortionExpression.Count > 0)
                        {
                            foreach (var item in search.listTytFetusAbortionExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        int start = param.Start.HasValue ? param.Start.Value : 0;
                        int limit = param.Limit.HasValue ? param.Limit.Value : Int32.MaxValue;
                        list = query.OrderByProperty(search.OrderField, search.OrderDirection).Skip(start).Take(limit).ToList();
                        param.Count = (from r in query select r).Count();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param), LogType.Error);
                LogSystem.Error(ex);
                list.Clear();
            }
            return list;
        }

        public TYT_FETUS_ABORTION GetById(long id, TytFetusAbortionSO search)
        {
            TYT_FETUS_ABORTION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.TYT_FETUS_ABORTION.AsQueryable().Where(p => p.ID == id);
                        if (search.listTytFetusAbortionExpression != null && search.listTytFetusAbortionExpression.Count > 0)
                        {
                            foreach (var item in search.listTytFetusAbortionExpression)
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
