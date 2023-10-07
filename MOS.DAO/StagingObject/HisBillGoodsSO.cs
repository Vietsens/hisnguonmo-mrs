using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBillGoodsSO : StagingObjectBase
    {
        public HisBillGoodsSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BILL_GOODS, bool>>> listHisBillGoodsExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BILL_GOODS, bool>>>();
    }
}
