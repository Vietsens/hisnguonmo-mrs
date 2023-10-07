using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaNationalSO : StagingObjectBase
    {
        public SdaNationalSO()
        {
            listSdaNationalExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSdaNationalExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_NATIONAL, bool>>> listSdaNationalExpression = new List<System.Linq.Expressions.Expression<Func<SDA_NATIONAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_NATIONAL, bool>>> listVSdaNationalExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_NATIONAL, bool>>>();
    }
}
