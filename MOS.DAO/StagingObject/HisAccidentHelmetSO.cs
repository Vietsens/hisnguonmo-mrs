using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentHelmetSO : StagingObjectBase
    {
        public HisAccidentHelmetSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HELMET, bool>>> listHisAccidentHelmetExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HELMET, bool>>>();
    }
}
