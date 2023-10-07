using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRehaSumSO : StagingObjectBase
    {
        public HisRehaSumSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REHA_SUM, bool>>> listHisRehaSumExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_SUM, bool>>>();
    }
}
