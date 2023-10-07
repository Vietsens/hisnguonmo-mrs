using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisIcdGroupSO : StagingObjectBase
    {
        public HisIcdGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ICD_GROUP, bool>>> listHisIcdGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ICD_GROUP, bool>>>();
    }
}
