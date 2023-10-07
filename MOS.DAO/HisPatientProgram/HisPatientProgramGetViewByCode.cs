using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPatientProgram
{
    partial class HisPatientProgramGet : EntityBase
    {
        public V_HIS_PATIENT_PROGRAM GetViewByCode(string code, HisPatientProgramSO search)
        {
            V_HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_PATIENT_PROGRAM.AsQueryable().Where(p => p.PATIENT_PROGRAM_CODE == code);
                        if (search.listVHisPatientProgramExpression != null && search.listVHisPatientProgramExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisPatientProgramExpression)
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
