using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisIcdCmSO : StagingObjectBase
    {
        public HisIcdCmSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ICD_CM, bool>>> listHisIcdCmExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ICD_CM, bool>>>();
    }
}
