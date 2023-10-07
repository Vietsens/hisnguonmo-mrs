using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediStockSO : StagingObjectBase
    {
        public HisMediStockSO()
        {
            //listHisMediStockExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMediStockExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK, bool>>> listHisMediStockExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK, bool>>> listVHisMediStockExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK, bool>>>();
    }
}
