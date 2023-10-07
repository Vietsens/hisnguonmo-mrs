using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediReactSO : StagingObjectBase
    {
        public HisMediReactSO()
        {
            //listHisMediReactExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMediReactExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT, bool>>> listHisMediReactExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT, bool>>> listVHisMediReactExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT, bool>>>();
    }
}
