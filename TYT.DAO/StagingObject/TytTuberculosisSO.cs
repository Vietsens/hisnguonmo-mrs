using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytTuberculosisSO : StagingObjectBase
    {
        public TytTuberculosisSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_TUBERCULOSIS, bool>>> listTytTuberculosisExpression = new List<System.Linq.Expressions.Expression<Func<TYT_TUBERCULOSIS, bool>>>();
    }
}
