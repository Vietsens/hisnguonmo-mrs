using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockPeriodSO : StagingObjectBase
    {
        public HisMediStockPeriodSO()
        {
            //listHisMediStockPeriodExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMediStockPeriodExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_PERIOD, bool>>> listHisMediStockPeriodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_PERIOD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_PERIOD, bool>>> listVHisMediStockPeriodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_PERIOD, bool>>>();
    }
}
