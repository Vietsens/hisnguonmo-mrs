using SDA.DAO.Base;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.DAO.StagingObject
{
    public class SdaCommuneSO : StagingObjectBase
    {
        public SdaCommuneSO()
        {
            listSdaCommuneExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVSdaCommuneExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<SDA_COMMUNE, bool>>> listSdaCommuneExpression = new List<System.Linq.Expressions.Expression<Func<SDA_COMMUNE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_SDA_COMMUNE, bool>>> listVSdaCommuneExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_COMMUNE, bool>>>();
    }
}
