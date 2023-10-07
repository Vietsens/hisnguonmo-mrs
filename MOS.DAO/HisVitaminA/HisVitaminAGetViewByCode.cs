using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisVitaminA
{
    partial class HisVitaminAGet : EntityBase
    {
        public V_HIS_VITAMIN_A GetViewByCode(string code, HisVitaminASO search)
        {
            V_HIS_VITAMIN_A result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_VITAMIN_A.AsQueryable().Where(p => p.VITAMIN_A_CODE == code);
                        if (search.listVHisVitaminAExpression != null && search.listVHisVitaminAExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisVitaminAExpression)
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
