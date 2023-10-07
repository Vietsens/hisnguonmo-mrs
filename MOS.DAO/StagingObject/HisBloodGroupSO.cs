using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBloodGroupSO : StagingObjectBase
    {
        public HisBloodGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_GROUP, bool>>> listHisBloodGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_GROUP, bool>>>();
    }
}
