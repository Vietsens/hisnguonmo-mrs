using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPtttCatastrophe
{
    partial class HisPtttCatastropheGet : EntityBase
    {
        public HIS_PTTT_CATASTROPHE GetByCode(string code, HisPtttCatastropheSO search)
        {
            HIS_PTTT_CATASTROPHE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_PTTT_CATASTROPHE.AsQueryable().Where(p => p.PTTT_CATASTROPHE_CODE == code);
                        if (search.listHisPtttCatastropheExpression != null && search.listHisPtttCatastropheExpression.Count > 0)
                        {
                            foreach (var item in search.listHisPtttCatastropheExpression)
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
