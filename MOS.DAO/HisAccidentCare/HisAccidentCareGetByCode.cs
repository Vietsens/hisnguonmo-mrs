using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAccidentCare
{
    partial class HisAccidentCareGet : EntityBase
    {
        public HIS_ACCIDENT_CARE GetByCode(string code, HisAccidentCareSO search)
        {
            HIS_ACCIDENT_CARE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_ACCIDENT_CARE.AsQueryable().Where(p => p.ACCIDENT_CARE_CODE == code);
                        if (search.listHisAccidentCareExpression != null && search.listHisAccidentCareExpression.Count > 0)
                        {
                            foreach (var item in search.listHisAccidentCareExpression)
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
