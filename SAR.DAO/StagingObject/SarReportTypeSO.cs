using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SAR.DAO.StagingObject
{
    public class SarReportTypeSO : StagingObjectBase
    {
        public SarReportTypeSO()
        {
            listSarReportTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SAR_REPORT_TYPE, bool>>> listSarReportTypeExpression = new List<System.Linq.Expressions.Expression<Func<SAR_REPORT_TYPE, bool>>>();
    }
}
