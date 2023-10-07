using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisImpMestType
{
    partial class HisImpMestTypeGet : EntityBase
    {
        public HIS_IMP_MEST_TYPE GetByCode(string code, HisImpMestTypeSO search)
        {
            HIS_IMP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_IMP_MEST_TYPE.AsQueryable().Where(p => p.IMP_MEST_TYPE_CODE == code);
                        if (search.listHisImpMestTypeExpression != null && search.listHisImpMestTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listHisImpMestTypeExpression)
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
