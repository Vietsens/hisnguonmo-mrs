using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using TYT.Filter;
using TYT.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytGdsk.Get
{
    public class TytGdskFilterQuery : TytGdskFilter
    {
        public TytGdskFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<TYT_GDSK, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<TYT_GDSK, bool>>>();

        internal Inventec.Backend.MANAGER.OrderProcessorBase OrderProcess = new Inventec.Backend.MANAGER.OrderProcessorBase();

        internal TytGdskSO Query()
        {
            TytGdskSO search = new TytGdskSO();
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
                if (this.GDSK_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.GDSK_TIME.Value >= this.GDSK_TIME_FROM.Value);
                }
                if (this.GDSK_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.GDSK_TIME.Value <= this.GDSK_TIME_TO.Value);
                }

                search.listTytGdskExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<TYT_GDSK>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listTytGdskExpression.Clear();
                search.listTytGdskExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
