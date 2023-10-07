using TYT.DAO.Base;
using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TYT.DAO.TytUninfectIcdGroup
{
    partial class TytUninfectIcdGroupGet : EntityBase
    {
        public List<V_TYT_UNINFECT_ICD_GROUP> GetView(TytUninfectIcdGroupSO search, CommonParam param)
        {
            List<V_TYT_UNINFECT_ICD_GROUP> list = new List<V_TYT_UNINFECT_ICD_GROUP>();
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_TYT_UNINFECT_ICD_GROUP.AsQueryable();
                        if (search.listVTytUninfectIcdGroupExpression != null && search.listVTytUninfectIcdGroupExpression.Count > 0)
                        {
                            foreach (var item in search.listVTytUninfectIcdGroupExpression)
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
    }
}
