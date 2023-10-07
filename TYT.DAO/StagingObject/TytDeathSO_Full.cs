using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytDeathSO : StagingObjectBase
    {
        public TytDeathSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_DEATH, bool>>> listTytDeathExpression = new List<System.Linq.Expressions.Expression<Func<TYT_DEATH, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_DEATH, bool>>> listVTytDeathExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_DEATH, bool>>>();
    }
}
