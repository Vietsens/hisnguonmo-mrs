using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExecuteGroupSO : StagingObjectBase
    {
        public HisExecuteGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_GROUP, bool>>> listHisExecuteGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_GROUP, bool>>>();
    }
}
