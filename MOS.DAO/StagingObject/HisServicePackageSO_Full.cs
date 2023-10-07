using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServicePackageSO : StagingObjectBase
    {
        public HisServicePackageSO()
        {
            //listHisServicePackageExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisServicePackageExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PACKAGE, bool>>> listHisServicePackageExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PACKAGE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PACKAGE, bool>>> listVHisServicePackageExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PACKAGE, bool>>>();
    }
}
