using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestInveUserSO : StagingObjectBase
    {
        public HisMestInveUserSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVE_USER, bool>>> listHisMestInveUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVE_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_INVE_USER, bool>>> listVHisMestInveUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_INVE_USER, bool>>>();
    }
}
