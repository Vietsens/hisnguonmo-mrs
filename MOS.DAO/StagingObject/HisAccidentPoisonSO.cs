using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentPoisonSO : StagingObjectBase
    {
        public HisAccidentPoisonSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_POISON, bool>>> listHisAccidentPoisonExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_POISON, bool>>>();
    }
}
