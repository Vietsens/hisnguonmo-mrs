using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBedTypeSO : StagingObjectBase
    {
        public HisBedTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BED_TYPE, bool>>> listHisBedTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED_TYPE, bool>>>();
    }
}
