using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisSuimIndex
{
    partial class HisSuimIndexGet : EntityBase
    {
        public V_HIS_SUIM_INDEX GetViewByCode(string code, HisSuimIndexSO search)
        {
            V_HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_SUIM_INDEX.AsQueryable().Where(p => p.SUIM_INDEX_CODE == code);
                        if (search.listVHisSuimIndexExpression != null && search.listVHisSuimIndexExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisSuimIndexExpression)
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
