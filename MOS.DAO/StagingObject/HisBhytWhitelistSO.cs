using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBhytWhitelistSO : StagingObjectBase
    {
        public HisBhytWhitelistSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BHYT_WHITELIST, bool>>> listHisBhytWhitelistExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BHYT_WHITELIST, bool>>>();
    }
}
