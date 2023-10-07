using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytFetusExamSO : StagingObjectBase
    {
        public TytFetusExamSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_FETUS_EXAM, bool>>> listTytFetusExamExpression = new List<System.Linq.Expressions.Expression<Func<TYT_FETUS_EXAM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_FETUS_EXAM, bool>>> listVTytFetusExamExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_FETUS_EXAM, bool>>>();
    }
}
