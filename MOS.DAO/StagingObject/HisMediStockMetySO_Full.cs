using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockMetySO : StagingObjectBase
    {
        public HisMediStockMetySO()
        {
            //listHisMediStockMetyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMediStockMetyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_METY, bool>>> listHisMediStockMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_METY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_METY, bool>>> listVHisMediStockMetyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_METY, bool>>>();
    }
}
