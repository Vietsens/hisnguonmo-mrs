using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSuimIndexSO : StagingObjectBase
    {
        public HisSuimIndexSO()
        {
            //listHisSuimIndexExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSuimIndexExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX, bool>>> listHisSuimIndexExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_INDEX, bool>>> listVHisSuimIndexExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_INDEX, bool>>>();
    }
}
