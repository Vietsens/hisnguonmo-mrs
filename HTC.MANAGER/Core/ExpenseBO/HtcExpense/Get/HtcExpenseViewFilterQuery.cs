using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using HTC.Filter;
using HTC.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get
{
    public class HtcExpenseViewFilterQuery : HtcExpenseViewFilter
    {
        public HtcExpenseViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HTC_EXPENSE, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<V_HTC_EXPENSE, bool>>>();

        internal Inventec.Backend.MANAGER.OrderProcessorBase OrderProcess = new Inventec.Backend.MANAGER.OrderProcessorBase();

        internal HtcExpenseSO Query()
        {
            HtcExpenseSO search = new HtcExpenseSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_FROM__GREATER.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value > this.CREATE_TIME_FROM__GREATER.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.CREATE_TIME_TO__LESS.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value < this.CREATE_TIME_TO__LESS.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_FROM__GREATER.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value > this.MODIFY_TIME_FROM__GREATER.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_TO__LESS.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value < this.MODIFY_TIME_TO__LESS.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null && this.IDs.Count > 0)
                {
                    listExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.PERIOD_ID.HasValue)
                {
                    listExpression.Add(o => o.PERIOD_ID == this.PERIOD_ID.Value);
                }

                if (this.EXPENSE_TYPE_ID.HasValue)
                {
                    listExpression.Add(o => o.EXPENSE_TYPE_ID == this.EXPENSE_TYPE_ID.Value);
                }

                if (this.PERIOD_IDs != null && this.PERIOD_IDs.Count > 0)
                {
                    listExpression.Add(o => this.PERIOD_IDs.Contains(o.PERIOD_ID));
                }

                if (this.EXPENSE_TYPE_IDs != null && this.EXPENSE_TYPE_IDs.Count > 0)
                {
                    listExpression.Add(o => this.EXPENSE_TYPE_IDs.Contains(o.EXPENSE_TYPE_ID));
                }

                if (this.EXPENSE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.EXPENSE_TIME >= this.EXPENSE_TIME_FROM.Value);
                }
                if (this.EXPENSE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.EXPENSE_TIME <= this.EXPENSE_TIME_TO.Value);
                }

                search.listVHtcExpenseExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<V_HTC_EXPENSE>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHtcExpenseExpression.Clear();
                search.listVHtcExpenseExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
