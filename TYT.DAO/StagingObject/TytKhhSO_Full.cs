using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytKhhSO : StagingObjectBase
    {
        public TytKhhSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_KHH, bool>>> listTytKhhExpression = new List<System.Linq.Expressions.Expression<Func<TYT_KHH, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_KHH, bool>>> listVTytKhhExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_KHH, bool>>>();
    }
}
