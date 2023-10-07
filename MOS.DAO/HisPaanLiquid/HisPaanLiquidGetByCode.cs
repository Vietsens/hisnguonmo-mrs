using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPaanLiquid
{
    partial class HisPaanLiquidGet : EntityBase
    {
        public HIS_PAAN_LIQUID GetByCode(string code, HisPaanLiquidSO search)
        {
            HIS_PAAN_LIQUID result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_PAAN_LIQUID.AsQueryable().Where(p => p.PAAN_LIQUID_CODE == code);
                        if (search.listHisPaanLiquidExpression != null && search.listHisPaanLiquidExpression.Count > 0)
                        {
                            foreach (var item in search.listHisPaanLiquidExpression)
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
