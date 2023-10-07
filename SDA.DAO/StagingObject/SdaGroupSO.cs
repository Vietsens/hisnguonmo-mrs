using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaGroupSO : StagingObjectBase
    {
        public SdaGroupSO()
        {
            listSdaGroupExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSdaGroupExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_GROUP, bool>>> listSdaGroupExpression = new List<System.Linq.Expressions.Expression<Func<SDA_GROUP, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_GROUP, bool>>> listVSdaGroupExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_GROUP, bool>>>();
    }
}
