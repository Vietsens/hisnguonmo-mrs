using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMaterialSO : StagingObjectBase
    {
        public HisMaterialSO()
        {
            //listHisMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL, bool>>> listHisMaterialExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL, bool>>> listVHisMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_1, bool>>> listVHisMaterial1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_2, bool>>> listVHisMaterial2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_2, bool>>>();
    }
}
