using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBornTypeSO : StagingObjectBase
    {
        public HisBornTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BORN_TYPE, bool>>> listHisBornTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BORN_TYPE, bool>>>();
    }
}
