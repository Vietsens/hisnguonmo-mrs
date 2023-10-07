using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytMalariaSO : StagingObjectBase
    {
        public TytMalariaSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_MALARIA, bool>>> listTytMalariaExpression = new List<System.Linq.Expressions.Expression<Func<TYT_MALARIA, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_MALARIA, bool>>> listVTytMalariaExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_MALARIA, bool>>>();
    }
}
