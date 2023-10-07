using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTransactionSO : StagingObjectBase
    {
        public HisTransactionSO()
        {
            //listHisTransactionExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisTransactionExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION, bool>>> listHisTransactionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION, bool>>> listVHisTransactionExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION_5, bool>>> listVHisTransaction5Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION_5, bool>>>();
    }
}
