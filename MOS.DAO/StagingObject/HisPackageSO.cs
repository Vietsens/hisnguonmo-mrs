using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPackageSO : StagingObjectBase
    {
        public HisPackageSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PACKAGE, bool>>> listHisPackageExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PACKAGE, bool>>>();
    }
}
