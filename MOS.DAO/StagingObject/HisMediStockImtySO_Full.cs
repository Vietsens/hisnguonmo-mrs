using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockImtySO : StagingObjectBase
    {
        public HisMediStockImtySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_IMTY, bool>>> listHisMediStockImtyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_IMTY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_IMTY, bool>>> listVHisMediStockImtyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_IMTY, bool>>>();
    }
}
