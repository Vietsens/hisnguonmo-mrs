using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaDistrictSO : StagingObjectBase
    {
        public SdaDistrictSO()
        {
            listSdaDistrictExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSdaDistrictExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_DISTRICT, bool>>> listSdaDistrictExpression = new List<System.Linq.Expressions.Expression<Func<SDA_DISTRICT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_DISTRICT, bool>>> listVSdaDistrictExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_DISTRICT, bool>>>();
    }
}
