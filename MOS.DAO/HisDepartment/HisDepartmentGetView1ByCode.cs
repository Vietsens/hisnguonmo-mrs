using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDepartment
{
    partial class HisDepartmentGet : EntityBase
    {
        public V_HIS_DEPARTMENT_1 GetView1ByCode(string code, HisDepartmentSO search)
        {
            V_HIS_DEPARTMENT_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_DEPARTMENT_1.AsQueryable().Where(p => p.DEPARTMENT_CODE == code);
                        if (search.listVHisDepartment1Expression != null && search.listVHisDepartment1Expression.Count > 0)
                        {
                            foreach (var item in search.listVHisDepartment1Expression)
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
