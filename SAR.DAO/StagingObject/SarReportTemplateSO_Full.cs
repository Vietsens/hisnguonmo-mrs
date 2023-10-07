using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SAR.DAO.StagingObject
{
    public class SarReportTemplateSO : StagingObjectBase
    {
        public SarReportTemplateSO()
        {
            listSarReportTemplateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSarReportTemplateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SAR_REPORT_TEMPLATE, bool>>> listSarReportTemplateExpression = new List<System.Linq.Expressions.Expression<Func<SAR_REPORT_TEMPLATE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT_TEMPLATE, bool>>> listVSarReportTemplateExpression = new List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT_TEMPLATE, bool>>>();
    }
}
