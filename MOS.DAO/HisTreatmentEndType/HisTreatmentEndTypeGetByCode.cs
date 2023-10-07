using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTreatmentEndType
{
    partial class HisTreatmentEndTypeGet : EntityBase
    {
        public HIS_TREATMENT_END_TYPE GetByCode(string code, HisTreatmentEndTypeSO search)
        {
            HIS_TREATMENT_END_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_TREATMENT_END_TYPE.AsQueryable().Where(p => p.TREATMENT_END_TYPE_CODE == code);
                        if (search.listHisTreatmentEndTypeExpression != null && search.listHisTreatmentEndTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listHisTreatmentEndTypeExpression)
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
