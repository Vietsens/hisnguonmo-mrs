using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBabySO : StagingObjectBase
    {
        public HisBabySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BABY, bool>>> listHisBabyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BABY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BABY, bool>>> listVHisBabyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BABY, bool>>>();
    }
}
