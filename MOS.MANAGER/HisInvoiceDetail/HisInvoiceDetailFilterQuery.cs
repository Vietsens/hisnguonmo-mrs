using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;

namespace MOS.MANAGER.HisInvoiceDetail
{
    public class HisInvoiceDetailFilterQuery : HisInvoiceDetailFilter
    {
        public HisInvoiceDetailFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_DETAIL, bool>>> listHisInvoiceDetailExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INVOICE_DETAIL, bool>>>();

        

        internal HisInvoiceDetailSO Query()
        {
            HisInvoiceDetailSO search = new HisInvoiceDetailSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisInvoiceDetailExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisInvoiceDetailExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisInvoiceDetailExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisInvoiceDetailExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisInvoiceDetailExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.INVOICE_ID.HasValue)
                {
                    listHisInvoiceDetailExpression.Add(o => o.INVOICE_ID == this.INVOICE_ID.Value);
                }
                if (this.INVOICE_IDs != null)
                {
                    listHisInvoiceDetailExpression.Add(o => this.INVOICE_IDs.Contains(o.INVOICE_ID));
                }

                search.listHisInvoiceDetailExpression.AddRange(listHisInvoiceDetailExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisInvoiceDetailExpression.Clear();
                search.listHisInvoiceDetailExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
