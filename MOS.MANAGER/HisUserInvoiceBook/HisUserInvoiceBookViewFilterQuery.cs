using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserInvoiceBook
{
    public class HisUserInvoiceBookViewFilterQuery : HisUserInvoiceBookViewFilter
    {
        public HisUserInvoiceBookViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_USER_INVOICE_BOOK, bool>>> listVHisUserInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_USER_INVOICE_BOOK, bool>>>();

        

        internal HisUserInvoiceBookSO Query()
        {
            HisUserInvoiceBookSO search = new HisUserInvoiceBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisUserInvoiceBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.LOGINNAME))
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.LOGINNAME == this.LOGINNAME);
                }
                if (this.INVOICE_BOOK_ID.HasValue)
                {
                    listVHisUserInvoiceBookExpression.Add(o => o.INVOICE_BOOK_ID == this.INVOICE_BOOK_ID.Value);
                }

                search.listVHisUserInvoiceBookExpression.AddRange(listVHisUserInvoiceBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisUserInvoiceBookExpression.Clear();
                search.listVHisUserInvoiceBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
