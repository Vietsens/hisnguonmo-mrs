using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInvoiceDetailSO : StagingObjectBase
    {
        public HisInvoiceDetailSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_DETAIL, bool>>> listHisInvoiceDetailExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_DETAIL, bool>>>();
    }
}
