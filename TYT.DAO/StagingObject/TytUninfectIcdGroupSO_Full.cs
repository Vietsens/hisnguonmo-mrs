using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytUninfectIcdGroupSO : StagingObjectBase
    {
        public TytUninfectIcdGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT_ICD_GROUP, bool>>> listTytUninfectIcdGroupExpression = new List<System.Linq.Expressions.Expression<Func<TYT_UNINFECT_ICD_GROUP, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_UNINFECT_ICD_GROUP, bool>>> listVTytUninfectIcdGroupExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_UNINFECT_ICD_GROUP, bool>>>();
    }
}
