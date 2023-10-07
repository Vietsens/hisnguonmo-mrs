using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDataStoreSO : StagingObjectBase
    {
        public HisDataStoreSO()
        {
            //listHisDataStoreExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisDataStoreExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DATA_STORE, bool>>> listHisDataStoreExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DATA_STORE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_DATA_STORE, bool>>> listVHisDataStoreExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DATA_STORE, bool>>>();
    }
}
