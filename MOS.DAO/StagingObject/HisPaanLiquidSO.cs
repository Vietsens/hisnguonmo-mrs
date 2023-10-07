using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPaanLiquidSO : StagingObjectBase
    {
        public HisPaanLiquidSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PAAN_LIQUID, bool>>> listHisPaanLiquidExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PAAN_LIQUID, bool>>>();
    }
}
