using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMilitaryRankSO : StagingObjectBase
    {
        public HisMilitaryRankSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MILITARY_RANK, bool>>> listHisMilitaryRankExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MILITARY_RANK, bool>>>();
    }
}
