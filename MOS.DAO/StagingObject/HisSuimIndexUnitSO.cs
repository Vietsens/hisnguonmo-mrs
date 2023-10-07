using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSuimIndexUnitSO : StagingObjectBase
    {
        public HisSuimIndexUnitSO()
        {
            //listHisSuimIndexUnitExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX_UNIT, bool>>> listHisSuimIndexUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX_UNIT, bool>>>();
    }
}
