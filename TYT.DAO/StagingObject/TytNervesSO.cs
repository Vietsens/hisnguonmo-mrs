using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytNervesSO : StagingObjectBase
    {
        public TytNervesSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_NERVES, bool>>> listTytNervesExpression = new List<System.Linq.Expressions.Expression<Func<TYT_NERVES, bool>>>();
    }
}
