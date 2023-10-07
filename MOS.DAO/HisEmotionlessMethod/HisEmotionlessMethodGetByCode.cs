using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisEmotionlessMethod
{
    partial class HisEmotionlessMethodGet : EntityBase
    {
        public HIS_EMOTIONLESS_METHOD GetByCode(string code, HisEmotionlessMethodSO search)
        {
            HIS_EMOTIONLESS_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EMOTIONLESS_METHOD.AsQueryable().Where(p => p.EMOTIONLESS_METHOD_CODE == code);
                        if (search.listHisEmotionlessMethodExpression != null && search.listHisEmotionlessMethodExpression.Count > 0)
                        {
                            foreach (var item in search.listHisEmotionlessMethodExpression)
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
