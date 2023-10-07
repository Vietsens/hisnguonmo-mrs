using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBloodAboSO : StagingObjectBase
    {
        public HisBloodAboSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_ABO, bool>>> listHisBloodAboExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_ABO, bool>>>();
    }
}
