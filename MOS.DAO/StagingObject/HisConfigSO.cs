using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisConfigSO : StagingObjectBase
    {
        public HisConfigSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CONFIG, bool>>> listHisConfigExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CONFIG, bool>>>();
    }
}
