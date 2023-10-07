using SAR.DAO.Base;
using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAR.DAO.SarReportTemplate
{
    partial class SarReportTemplateGet : EntityBase
    {
        public V_SAR_REPORT_TEMPLATE GetViewByCode(string code, SarReportTemplateSO search)
        {
            V_SAR_REPORT_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.V_SAR_REPORT_TEMPLATE.AsQueryable().Where(p => p.REPORT_TEMPLATE_CODE == code);
                        if (search.listVSarReportTemplateExpression != null && search.listVSarReportTemplateExpression.Count > 0)
                        {
                            foreach (var item in search.listVSarReportTemplateExpression)
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
