using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDepartmentTranSO : StagingObjectBase
    {
        public HisDepartmentTranSO()
        {
            //listHisDepartmentTranExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisDepartmentTranExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT_TRAN, bool>>> listHisDepartmentTranExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT_TRAN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_TRAN, bool>>> listVHisDepartmentTranExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_TRAN, bool>>>();
    }
}
