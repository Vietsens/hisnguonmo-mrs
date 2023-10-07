using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisImpMestStt
{
    partial class HisImpMestSttGet : EntityBase
    {
        public HIS_IMP_MEST_STT GetByCode(string code, HisImpMestSttSO search)
        {
            HIS_IMP_MEST_STT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_IMP_MEST_STT.AsQueryable().Where(p => p.IMP_MEST_STT_CODE == code);
                        if (search.listHisImpMestSttExpression != null && search.listHisImpMestSttExpression.Count > 0)
                        {
                            foreach (var item in search.listHisImpMestSttExpression)
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
