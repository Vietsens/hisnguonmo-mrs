using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInvoiceBookSO : StagingObjectBase
    {
        public HisInvoiceBookSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_BOOK, bool>>> listHisInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_BOOK, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE_BOOK, bool>>> listVHisInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE_BOOK, bool>>>();
    }
}
