using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpSourceSO : StagingObjectBase
    {
        public HisImpSourceSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_SOURCE, bool>>> listHisImpSourceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_SOURCE, bool>>>();
    }
}
