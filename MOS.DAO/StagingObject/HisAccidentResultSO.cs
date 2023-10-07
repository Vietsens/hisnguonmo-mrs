using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentResultSO : StagingObjectBase
    {
        public HisAccidentResultSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_RESULT, bool>>> listHisAccidentResultExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_RESULT, bool>>>();
    }
}
