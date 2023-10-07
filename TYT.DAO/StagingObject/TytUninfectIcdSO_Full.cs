using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytUninfectIcdSO : StagingObjectBase
    {
        public TytUninfectIcdSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT_ICD, bool>>> listTytUninfectIcdExpression = new List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT_ICD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_UNINFECT_ICD, bool>>> listVTytUninfectIcdExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_UNINFECT_ICD, bool>>>();
    }
}
