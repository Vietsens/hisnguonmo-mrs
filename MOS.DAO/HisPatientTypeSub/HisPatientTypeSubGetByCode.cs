using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPatientTypeSub
{
    partial class HisPatientTypeSubGet : EntityBase
    {
        public HIS_PATIENT_TYPE_SUB GetByCode(string code, HisPatientTypeSubSO search)
        {
            HIS_PATIENT_TYPE_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_PATIENT_TYPE_SUB.AsQueryable().Where(p => p.PATIENT_TYPE_SUB_CODE == code);
                        if (search.listHisPatientTypeSubExpression != null && search.listHisPatientTypeSubExpression.Count > 0)
                        {
                            foreach (var item in search.listHisPatientTypeSubExpression)
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
