using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisBloodType
{
    partial class HisBloodTypeGet : EntityBase
    {
        public V_HIS_BLOOD_TYPE GetViewByCode(string code, HisBloodTypeSO search)
        {
            V_HIS_BLOOD_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_BLOOD_TYPE.AsQueryable().Where(p => p.BLOOD_TYPE_CODE == code);
                        if (search.listVHisBloodTypeExpression != null && search.listVHisBloodTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisBloodTypeExpression)
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
