using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMaterialPatySO : StagingObjectBase
    {
        public HisMaterialPatySO()
        {
            //listHisMaterialPatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMaterialPatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_PATY, bool>>> listHisMaterialPatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_PATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_PATY, bool>>> listVHisMaterialPatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_PATY, bool>>>();
    }
}
