using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockMatySO : StagingObjectBase
    {
        public HisMediStockMatySO()
        {
            //listHisMediStockMatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMediStockMatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_MATY, bool>>> listHisMediStockMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_MATY, bool>>> listVHisMediStockMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_MATY, bool>>>();
    }
}
