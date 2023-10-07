using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDepartmentSO : StagingObjectBase
    {
        public HisDepartmentSO()
        {
            //listHisDepartmentExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT, bool>>> listHisDepartmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT, bool>>> listVHisDepartmentExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_1, bool>>> listVHisDepartment1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_1, bool>>>();
    }
}
