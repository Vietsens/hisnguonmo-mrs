using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisOtherPaySourceSO : StagingObjectBase
    {
        public HisOtherPaySourceSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_OTHER_PAY_SOURCE, bool>>> listHisOtherPaySourceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_OTHER_PAY_SOURCE, bool>>>();
    }
}
