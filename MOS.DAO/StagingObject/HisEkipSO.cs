using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEkipSO : StagingObjectBase
    {
        public HisEkipSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EKIP, bool>>> listHisEkipExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EKIP, bool>>>();
    }
}
