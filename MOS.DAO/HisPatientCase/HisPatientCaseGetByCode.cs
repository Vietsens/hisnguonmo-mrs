using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisPatientCase
{
    partial class HisPatientCaseGet : EntityBase
    {
        public HIS_PATIENT_CASE GetByCode(string code, HisPatientCaseSO search)
        {
            HIS_PATIENT_CASE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_PATIENT_CASE.AsQueryable().Where(p => p.PATIENT_CASE_CODE == code);
                        if (search.listHisPatientCaseExpression != null && search.listHisPatientCaseExpression.Count > 0)
                        {
                            foreach (var item in search.listHisPatientCaseExpression)
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
