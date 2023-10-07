using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserInvoiceBook
{
    public class HisUserInvoiceBookFilterQuery : HisUserInvoiceBookFilter
    {
        public HisUserInvoiceBookFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_USER_INVOICE_BOOK, bool>>> listHisUserInvoiceBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_USER_INVOICE_BOOK, bool>>>();

        

        internal HisUserInvoiceBookSO Query()
        {
            HisUserInvoiceBookSO search = new HisUserInvoiceBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisUserInvoiceBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisUserInvoiceBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisUserInvoiceBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisUserInvoiceBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.LOGINNAME))
                {
                    listHisUserInvoiceBookExpression.Add(o => o.LOGINNAME == this.LOGINNAME);
                }
                if (this.INVOICE_BOOK_ID.HasValue)
                {
                    listHisUserInvoiceBookExpression.Add(o => o.INVOICE_BOOK_ID == this.INVOICE_BOOK_ID.Value);
                }

                search.listHisUserInvoiceBookExpression.AddRange(listHisUserInvoiceBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisUserInvoiceBookExpression.Clear();
                search.listHisUserInvoiceBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
