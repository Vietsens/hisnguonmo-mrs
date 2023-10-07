using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediReactSumSO : StagingObjectBase
    {
        public HisMediReactSumSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT_SUM, bool>>> listHisMediReactSumExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT_SUM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT_SUM, bool>>> listVHisMediReactSumExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT_SUM, bool>>>();
    }
}
