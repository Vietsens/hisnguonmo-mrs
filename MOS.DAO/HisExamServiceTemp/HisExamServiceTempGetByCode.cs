using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExamServiceTemp
{
    partial class HisExamServiceTempGet : EntityBase
    {
        public HIS_EXAM_SERVICE_TEMP GetByCode(string code, HisExamServiceTempSO search)
        {
            HIS_EXAM_SERVICE_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EXAM_SERVICE_TEMP.AsQueryable().Where(p => p.EXAM_SERVICE_TEMP_CODE == code);
                        if (search.listHisExamServiceTempExpression != null && search.listHisExamServiceTempExpression.Count > 0)
                        {
                            foreach (var item in search.listHisExamServiceTempExpression)
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
