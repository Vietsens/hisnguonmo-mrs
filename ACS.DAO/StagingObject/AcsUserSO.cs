using ACS.DAO.Base;
using ACS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace ACS.DAO.StagingObject
{
    public class AcsUserSO : StagingObjectBase
    {
        public AcsUserSO()
        {
            listAcsUserExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<ACS_USER, bool>>> listAcsUserExpression = new List<System.Linq.Expressions.Expression<Func<ACS_USER, bool>>>();
    }
}
