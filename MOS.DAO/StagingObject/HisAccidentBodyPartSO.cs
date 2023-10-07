using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentBodyPartSO : StagingObjectBase
    {
        public HisAccidentBodyPartSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_BODY_PART, bool>>> listHisAccidentBodyPartExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_BODY_PART, bool>>>();
    }
}
