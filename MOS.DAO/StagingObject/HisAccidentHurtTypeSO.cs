using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentHurtTypeSO : StagingObjectBase
    {
        public HisAccidentHurtTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT_TYPE, bool>>> listHisAccidentHurtTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT_TYPE, bool>>>();
    }
}
