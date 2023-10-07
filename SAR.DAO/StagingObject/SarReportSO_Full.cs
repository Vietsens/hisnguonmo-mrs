using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SAR.DAO.StagingObject
{
    public class SarReportSO : StagingObjectBase
    {
        public SarReportSO()
        {
            listSarReportExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSarReportExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SAR_REPORT, bool>>> listSarReportExpression = new List<System.Linq.Expressions.Expression<Func<SAR_REPORT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT, bool>>> listVSarReportExpression = new List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT, bool>>>();
    }
}
