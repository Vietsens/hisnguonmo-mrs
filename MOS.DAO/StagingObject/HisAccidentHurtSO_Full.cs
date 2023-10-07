using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentHurtSO : StagingObjectBase
    {
        public HisAccidentHurtSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT, bool>>> listHisAccidentHurtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ACCIDENT_HURT, bool>>> listVHisAccidentHurtExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACCIDENT_HURT, bool>>>();
    }
}
