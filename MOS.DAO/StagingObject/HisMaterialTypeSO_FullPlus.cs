using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMaterialTypeSO : StagingObjectBase
    {
        public HisMaterialTypeSO()
        {
            //listHisMaterialTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMaterialTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_TYPE, bool>>> listHisMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_TYPE, bool>>> listVHisMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_TYPE, bool>>>();
    }
}
