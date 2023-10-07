using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisHeinServiceType
{
    partial class HisHeinServiceTypeGet : EntityBase
    {
        public HIS_HEIN_SERVICE_TYPE GetByCode(string code, HisHeinServiceTypeSO search)
        {
            HIS_HEIN_SERVICE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_HEIN_SERVICE_TYPE.AsQueryable().Where(p => p.HEIN_SERVICE_TYPE_CODE == code);
                        if (search.listHisHeinServiceTypeExpression != null && search.listHisHeinServiceTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listHisHeinServiceTypeExpression)
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
