using ACS.DAO.Base;
using ACS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace ACS.DAO.StagingObject
{
    public class AcsRoleSO : StagingObjectBase
    {
        public AcsRoleSO()
        {
            listAcsRoleExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<ACS_ROLE, bool>>> listAcsRoleExpression = new List<System.Linq.Expressions.Expression<Func<ACS_ROLE, bool>>>();
    }
}
