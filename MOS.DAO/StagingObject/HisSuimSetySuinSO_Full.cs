using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSuimSetySuinSO : StagingObjectBase
    {
        public HisSuimSetySuinSO()
        {
            //listHisSuimSetySuinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSuimSetySuinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SUIM_SETY_SUIN, bool>>> listHisSuimSetySuinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_SETY_SUIN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_SETY_SUIN, bool>>> listVHisSuimSetySuinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_SETY_SUIN, bool>>>();
    }
}
