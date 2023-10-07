using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytUninfectSO : StagingObjectBase
    {
        public TytUninfectSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT, bool>>> listTytUninfectExpression = new List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT, bool>>>();
    }
}
