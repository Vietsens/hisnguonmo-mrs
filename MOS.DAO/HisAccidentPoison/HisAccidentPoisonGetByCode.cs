using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAccidentPoison
{
    partial class HisAccidentPoisonGet : EntityBase
    {
        public HIS_ACCIDENT_POISON GetByCode(string code, HisAccidentPoisonSO search)
        {
            HIS_ACCIDENT_POISON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_ACCIDENT_POISON.AsQueryable().Where(p => p.ACCIDENT_POISON_CODE == code);
                        if (search.listHisAccidentPoisonExpression != null && search.listHisAccidentPoisonExpression.Count > 0)
                        {
                            foreach (var item in search.listHisAccidentPoisonExpression)
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
