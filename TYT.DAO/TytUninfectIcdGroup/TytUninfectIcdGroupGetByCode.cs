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
        public TYT_UNINFECT_ICD_GROUP GetByCode(string code, TytUninfectIcdGroupSO search)
        {
            TYT_UNINFECT_ICD_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.TYT_UNINFECT_ICD_GROUP.AsQueryable().Where(p => p.UNINFECT_ICD_GROUP_CODE == code);
                        if (search.listTytUninfectIcdGroupExpression != null && search.listTytUninfectIcdGroupExpression.Count > 0)
                        {
                            foreach (var item in search.listTytUninfectIcdGroupExpression)
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
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
