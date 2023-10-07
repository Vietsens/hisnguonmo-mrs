using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPaanPositionSO : StagingObjectBase
    {
        public HisPaanPositionSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PAAN_POSITION, bool>>> listHisPaanPositionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PAAN_POSITION, bool>>>();
    }
}
