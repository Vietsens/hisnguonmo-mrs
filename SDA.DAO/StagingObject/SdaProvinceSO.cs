using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaProvinceSO : StagingObjectBase
    {
        public SdaProvinceSO()
        {
            listSdaProvinceExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSdaProvinceExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_PROVINCE, bool>>> listSdaProvinceExpression = new List<System.Linq.Expressions.Expression<Func<SDA_PROVINCE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_PROVINCE, bool>>> listVSdaProvinceExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_PROVINCE, bool>>>();
    }
}
