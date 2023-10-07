using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceBook
{
    public class HisInvoiceBookFilterQuery : HisInvoiceBookFilter
    {
        public HisInvoiceBookFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_BOOK, bool>>> listHisInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_BOOK, bool>>>();

        

        internal HisInvoiceBookSO Query()
        {
            HisInvoiceBookSO search = new HisInvoiceBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisInvoiceBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisInvoiceBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisInvoiceBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisInvoiceBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisInvoiceBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.SYMBOL_CODE__EXACT))
                {
                    listHisInvoiceBookExpression.Add(o => o.SYMBOL_CODE == this.SYMBOL_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TEMPLATE_CODE__EXACT))
                {
                    listHisInvoiceBookExpression.Add(o => o.TEMPLATE_CODE == this.TEMPLATE_CODE__EXACT);
                }

                search.listHisInvoiceBookExpression.AddRange(listHisInvoiceBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisInvoiceBookExpression.Clear();
                search.listHisInvoiceBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
