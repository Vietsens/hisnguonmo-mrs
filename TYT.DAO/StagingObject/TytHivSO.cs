using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytHivSO : StagingObjectBase
    {
        public TytHivSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_HIV, bool>>> listTytHivExpression = new List<System.Linq.Expressions.Expression<Func<TYT_HIV, bool>>>();
    }
}
