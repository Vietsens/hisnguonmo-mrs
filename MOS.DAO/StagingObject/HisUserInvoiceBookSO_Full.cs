using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisUserInvoiceBookSO : StagingObjectBase
    {
        public HisUserInvoiceBookSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_USER_INVOICE_BOOK, bool>>> listHisUserInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_USER_INVOICE_BOOK, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_USER_INVOICE_BOOK, bool>>> listVHisUserInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_USER_INVOICE_BOOK, bool>>>();
    }
}
