using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBedSO : StagingObjectBase
    {
        public HisBedSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BED, bool>>> listHisBedExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BED, bool>>> listVHisBedExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BED, bool>>>();
    }
}
