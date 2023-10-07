using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisReportTypeCatSO : StagingObjectBase
    {
        public HisReportTypeCatSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REPORT_TYPE_CAT, bool>>> listHisReportTypeCatExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REPORT_TYPE_CAT, bool>>>();
    }
}
