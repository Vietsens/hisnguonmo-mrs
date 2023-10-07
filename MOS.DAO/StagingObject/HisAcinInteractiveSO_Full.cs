using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAcinInteractiveSO : StagingObjectBase
    {
        public HisAcinInteractiveSO()
        {
            //listHisAcinInteractiveExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisAcinInteractiveExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACIN_INTERACTIVE, bool>>> listHisAcinInteractiveExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACIN_INTERACTIVE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ACIN_INTERACTIVE, bool>>> listVHisAcinInteractiveExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACIN_INTERACTIVE, bool>>>();
    }
}
