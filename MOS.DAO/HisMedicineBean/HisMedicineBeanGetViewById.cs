using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisMedicineBean
{
    partial class HisMedicineBeanGet : EntityBase
    {
        public V_HIS_MEDICINE_BEAN GetViewById(long id, HisMedicineBeanSO search)
        {
            V_HIS_MEDICINE_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_MEDICINE_BEAN.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisMedicineBeanExpression != null && search.listVHisMedicineBeanExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisMedicineBeanExpression)
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
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => id), id) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
