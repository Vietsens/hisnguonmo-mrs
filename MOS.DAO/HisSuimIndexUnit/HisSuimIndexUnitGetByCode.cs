using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisSuimIndexUnit
{
    partial class HisSuimIndexUnitGet : EntityBase
    {
        public HIS_SUIM_INDEX_UNIT GetByCode(string code, HisSuimIndexUnitSO search)
        {
            HIS_SUIM_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_SUIM_INDEX_UNIT.AsQueryable().Where(p => p.SUIM_INDEX_UNIT_CODE == code);
                        if (search.listHisSuimIndexUnitExpression != null && search.listHisSuimIndexUnitExpression.Count > 0)
                        {
                            foreach (var item in search.listHisSuimIndexUnitExpression)
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
