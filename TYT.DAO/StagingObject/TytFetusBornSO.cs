using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytFetusBornSO : StagingObjectBase
    {
        public TytFetusBornSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_FETUS_BORN, bool>>> listTytFetusBornExpression = new List<System.Linq.Expressions.Expression<Func<TYT_FETUS_BORN, bool>>>();
    }
}
