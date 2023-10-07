using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBornPositionSO : StagingObjectBase
    {
        public HisBornPositionSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BORN_POSITION, bool>>> listHisBornPositionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BORN_POSITION, bool>>>();
    }
}
