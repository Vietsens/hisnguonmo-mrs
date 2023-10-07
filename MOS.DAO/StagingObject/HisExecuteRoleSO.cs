using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExecuteRoleSO : StagingObjectBase
    {
        public HisExecuteRoleSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE, bool>>> listHisExecuteRoleExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE, bool>>>();
    }
}
