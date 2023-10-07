using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEkipTempSO : StagingObjectBase
    {
        public HisEkipTempSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EKIP_TEMP, bool>>> listHisEkipTempExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EKIP_TEMP, bool>>>();
    }
}
