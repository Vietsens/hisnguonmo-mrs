using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisHtuSO : StagingObjectBase
    {
        public HisHtuSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_HTU, bool>>> listHisHtuExpression = new List<System.Linq.Expressions.Expression<Func<HIS_HTU, bool>>>();
    }
}
