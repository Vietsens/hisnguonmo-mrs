using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCareSumSO : StagingObjectBase
    {
        public HisCareSumSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CARE_SUM, bool>>> listHisCareSumExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARE_SUM, bool>>>();
    }
}
