using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCaroDepartmentSO : StagingObjectBase
    {
        public HisCaroDepartmentSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CARO_DEPARTMENT, bool>>> listHisCaroDepartmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARO_DEPARTMENT, bool>>>();
    }
}
