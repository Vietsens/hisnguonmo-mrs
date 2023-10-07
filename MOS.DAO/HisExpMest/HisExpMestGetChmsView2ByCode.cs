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
        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string code, HisExpMestSO search)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_EXP_MEST_CHMS_2.AsQueryable().Where(p => p.EXP_MEST_CODE == code);
                        if (search.listV2HisExpMestChmsExpression != null && search.listV2HisExpMestChmsExpression.Count > 0)
                        {
                            foreach (var item in search.listV2HisExpMestChmsExpression)
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
