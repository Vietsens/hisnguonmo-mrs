using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaConfigAppSO : StagingObjectBase
    {
        public SdaConfigAppSO()
        {
            listSdaConfigAppExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_CONFIG_APP, bool>>> listSdaConfigAppExpression = new List<System.Linq.Expressions.Expression<Func<SDA_CONFIG_APP, bool>>>();
    }
}
