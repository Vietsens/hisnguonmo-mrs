using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisSereServTemp
{
    partial class HisSereServTempGet : EntityBase
    {
        public HIS_SERE_SERV_TEMP GetByCode(string code, HisSereServTempSO search)
        {
            HIS_SERE_SERV_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_SERE_SERV_TEMP.AsQueryable().Where(p => p.SERE_SERV_TEMP_CODE == code);
                        if (search.listHisSereServTempExpression != null && search.listHisSereServTempExpression.Count > 0)
                        {
                            foreach (var item in search.listHisSereServTempExpression)
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
