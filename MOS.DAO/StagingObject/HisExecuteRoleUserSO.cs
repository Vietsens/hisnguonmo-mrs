using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExecuteRoleUserSO : StagingObjectBase
    {
        public HisExecuteRoleUserSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE_USER, bool>>> listHisExecuteRoleUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE_USER, bool>>>();
    }
}
