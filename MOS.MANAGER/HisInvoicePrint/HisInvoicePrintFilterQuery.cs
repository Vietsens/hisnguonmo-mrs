using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoicePrint
{
    public class HisInvoicePrintFilterQuery : HisInvoicePrintFilter
    {
        public HisInvoicePrintFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_PRINT, bool>>> listHisInvoicePrintExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_PRINT, bool>>>();

        

        internal HisInvoicePrintSO Query()
        {
            HisInvoicePrintSO search = new HisInvoicePrintSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisInvoicePrintExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisInvoicePrintExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisInvoicePrintExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisInvoicePrintExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisInvoicePrintExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.INVOICE_ID.HasValue)
                {
                    listHisInvoicePrintExpression.Add(o => o.INVOICE_ID == this.INVOICE_ID.Value);
                }
                
                search.listHisInvoicePrintExpression.AddRange(listHisInvoicePrintExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisInvoicePrintExpression.Clear();
                search.listHisInvoicePrintExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
