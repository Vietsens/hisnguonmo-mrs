using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInvoiceSO : StagingObjectBase
    {
        public HisInvoiceSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INVOICE, bool>>> listHisInvoiceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE, bool>>> listVHisInvoiceExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE, bool>>>();
    }
}
