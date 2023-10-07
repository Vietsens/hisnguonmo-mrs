using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockExtySO : StagingObjectBase
    {
        public HisMediStockExtySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_EXTY, bool>>> listHisMediStockExtyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_EXTY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_EXTY, bool>>> listVHisMediStockExtyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_EXTY, bool>>>();
    }
}
