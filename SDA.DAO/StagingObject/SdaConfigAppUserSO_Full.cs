using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaConfigAppUserSO : StagingObjectBase
    {
        public SdaConfigAppUserSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_CONFIG_APP_USER, bool>>> listSdaConfigAppUserExpression = new List<System.Linq.Expressions.Expression<Func<SDA_CONFIG_APP_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_CONFIG_APP_USER, bool>>> listVSdaConfigAppUserExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_CONFIG_APP_USER, bool>>>();
    }
}
