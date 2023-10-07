using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceBook
{
    public class HisInvoiceBookViewFilterQuery : HisInvoiceBookViewFilter
    {
        public HisInvoiceBookViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE_BOOK, bool>>> listVHisInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE_BOOK, bool>>>();

        

        internal HisInvoiceBookSO Query()
        {
            HisInvoiceBookSO search = new HisInvoiceBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisInvoiceBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisInvoiceBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisInvoiceBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisInvoiceBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.SYMBOL_CODE__EXACT))
                {
                    listVHisInvoiceBookExpression.Add(o => o.SYMBOL_CODE == this.SYMBOL_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TEMPLATE_CODE__EXACT))
                {
                    listVHisInvoiceBookExpression.Add(o => o.TEMPLATE_CODE == this.TEMPLATE_CODE__EXACT);
                }
                if (this.LINK_ID.HasValue)
                {
                    listVHisInvoiceBookExpression.Add(o => o.LINK_ID.HasValue && o.LINK_ID.Value == this.LINK_ID.Value);
                }
                if (this.LINK_IDs != null)
                {
                    listVHisInvoiceBookExpression.Add(o => o.LINK_ID.HasValue && this.LINK_IDs.Contains(o.LINK_ID.Value));
                }

                search.listVHisInvoiceBookExpression.AddRange(listVHisInvoiceBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisInvoiceBookExpression.Clear();
                search.listVHisInvoiceBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
