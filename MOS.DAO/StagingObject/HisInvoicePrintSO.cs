using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInvoicePrintSO : StagingObjectBase
    {
        public HisInvoicePrintSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_PRINT, bool>>> listHisInvoicePrintExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_PRINT, bool>>>();
    }
}
