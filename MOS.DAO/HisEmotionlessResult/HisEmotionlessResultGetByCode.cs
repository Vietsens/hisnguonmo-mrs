using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisEmotionlessResult
{
    partial class HisEmotionlessResultGet : EntityBase
    {
        public HIS_EMOTIONLESS_RESULT GetByCode(string code, HisEmotionlessResultSO search)
        {
            HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EMOTIONLESS_RESULT.AsQueryable().Where(p => p.EMOTIONLESS_RESULT_CODE == code);
                        if (search.listHisEmotionlessResultExpression != null && search.listHisEmotionlessResultExpression.Count > 0)
                        {
                            foreach (var item in search.listHisEmotionlessResultExpression)
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
