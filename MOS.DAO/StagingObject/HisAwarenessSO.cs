using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAwarenessSO : StagingObjectBase
    {
        public HisAwarenessSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_AWARENESS, bool>>> listHisAwarenessExpression = new List<System.Linq.Expressions.Expression<Func<HIS_AWARENESS, bool>>>();
    }
}
