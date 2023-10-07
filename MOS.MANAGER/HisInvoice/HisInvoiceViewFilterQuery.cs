using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoice
{
    public class HisInvoiceViewFilterQuery : HisInvoiceViewFilter
    {
        public HisInvoiceViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE, bool>>> listVHisInvoiceExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INVOICE, bool>>>();

        

        internal HisInvoiceSO Query()
        {
            HisInvoiceSO search = new HisInvoiceSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisInvoiceExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisInvoiceExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisInvoiceExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisInvoiceExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.INVOICE_BOOK_ID.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.INVOICE_BOOK_ID == this.INVOICE_BOOK_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.SYMBOL_CODE__EXACT))
                {
                    listVHisInvoiceExpression.Add(o => o.SYMBOL_CODE == this.SYMBOL_CODE__EXACT);
                }
                if (this.CREATORs != null)
                {
                    listVHisInvoiceExpression.Add(o => this.CREATORs.Contains(o.CREATOR));
                }
                if (this.INVOICE_TIME_FROM.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.INVOICE_TIME >= this.INVOICE_TIME_FROM.Value);
                }
                if (this.INVOICE_TIME_TO.HasValue)
                {
                    listVHisInvoiceExpression.Add(o => o.INVOICE_TIME <= this.INVOICE_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisInvoiceExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        //o.SECURITY_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SYMBOL_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TEMPLATE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.VIR_NUM_ORDER.ToLower().Contains(this.KEY_WORD) ||
                        (o.SELLER_ACCOUNT_NUMBER != null && o.SELLER_ACCOUNT_NUMBER.ToLower().Contains(this.KEY_WORD)) ||
                        (o.SELLER_ADDRESS != null && o.SELLER_ADDRESS.ToLower().Contains(this.KEY_WORD)) ||
                        (o.SELLER_NAME != null && o.SELLER_NAME.ToLower().Contains(this.KEY_WORD)) ||
                        (o.SELLER_PHONE != null && o.SELLER_PHONE.ToLower().Contains(this.KEY_WORD)) ||
                        (o.SELLER_TAX_CODE != null && o.SELLER_TAX_CODE.ToLower().Contains(this.KEY_WORD)) ||
                        (o.BUYER_ACCOUNT_NUMBER != null && o.BUYER_ACCOUNT_NUMBER.ToLower().Contains(this.KEY_WORD)) ||
                        (o.BUYER_ADDRESS != null && o.BUYER_ADDRESS.ToLower().Contains(this.KEY_WORD)) ||
                        (o.BUYER_NAME != null && o.BUYER_NAME.ToLower().Contains(this.KEY_WORD)) ||
                        (o.BUYER_ORGANIZATION != null && o.BUYER_ORGANIZATION.ToLower().Contains(this.KEY_WORD)) ||
                        (o.BUYER_TAX_CODE != null && o.BUYER_TAX_CODE.ToLower().Contains(this.KEY_WORD)) ||
                        (o.CANCEL_LOGINNAME != null && o.CANCEL_LOGINNAME.ToLower().Contains(this.KEY_WORD)) ||
                        (o.CANCEL_REASON != null && o.CANCEL_REASON.ToLower().Contains(this.KEY_WORD)) ||
                        (o.CANCEL_USERNAME != null && o.CANCEL_USERNAME.ToLower().Contains(this.KEY_WORD))
                        );
                }

                search.listVHisInvoiceExpression.AddRange(listVHisInvoiceExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisInvoiceExpression.Clear();
                search.listVHisInvoiceExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
