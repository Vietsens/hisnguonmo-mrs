using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDepositReq
{
    partial class HisDepositReqGet : EntityBase
    {
        public HIS_DEPOSIT_REQ GetByCode(string code, HisDepositReqSO search)
        {
            HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_DEPOSIT_REQ.AsQueryable().Where(p => p.DEPOSIT_REQ_CODE == code);
                        if (search.listHisDepositReqExpression != null && search.listHisDepositReqExpression.Count > 0)
                        {
                            foreach (var item in search.listHisDepositReqExpression)
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
