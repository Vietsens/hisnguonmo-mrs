using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediReactTypeSO : StagingObjectBase
    {
        public HisMediReactTypeSO()
        {
            //listHisMediReactTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT_TYPE, bool>>> listHisMediReactTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT_TYPE, bool>>>();
    }
}
