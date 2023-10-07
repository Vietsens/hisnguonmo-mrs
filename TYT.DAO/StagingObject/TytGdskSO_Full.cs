using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytGdskSO : StagingObjectBase
    {
        public TytGdskSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_GDSK, bool>>> listTytGdskExpression = new List<System.Linq.Expressions.Expression<Func<TYT_GDSK, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_GDSK, bool>>> listVTytGdskExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_GDSK, bool>>>();
    }
}
