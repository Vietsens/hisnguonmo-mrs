using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExpMest
{
    partial class HisExpMestGet : EntityBase
    {
        public HIS_EXP_MEST GetByCode(string code, HisExpMestSO search)
        {
            HIS_EXP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EXP_MEST.AsQueryable().Where(p => p.EXP_MEST_CODE == code);
                        if (search.listHisExpMestExpression != null && search.listHisExpMestExpression.Count > 0)
                        {
                            foreach (var item in search.listHisExpMestExpression)
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
