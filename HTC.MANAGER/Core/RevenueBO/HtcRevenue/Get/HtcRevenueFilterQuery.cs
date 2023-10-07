using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using HTC.Filter;
using HTC.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get
{
    public class HtcRevenueFilterQuery : HtcRevenueFilter
    {
        public HtcRevenueFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HTC_REVENUE, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<HTC_REVENUE, bool>>>();

        internal Inventec.Backend.MANAGER.OrderProcessorBase OrderProcess = new Inventec.Backend.MANAGER.OrderProcessorBase();

        internal HtcRevenueSO Query()
        {
            HtcRevenueSO search = new HtcRevenueSO();
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

                if (this.PERIOD_IDs != null && this.PERIOD_IDs.Count > 0)
                {
                    listExpression.Add(o => this.PERIOD_IDs.Contains(o.PERIOD_ID));
                }

                if (this.REVENUE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.REVENUE_TIME >= this.REVENUE_TIME_FROM.Value);
                }

                if (this.REVENUE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.REVENUE_TIME <= this.REVENUE_TIME_TO.Value);
                }

                if (this.IN_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.IN_TIME.Value >= this.IN_TIME_FROM.Value);
                }

                if (this.IN_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.IN_TIME.Value <= this.IN_TIME_TO.Value);
                }

                if (this.OUT_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.OUT_TIME.Value >= this.OUT_TIME_FROM.Value);
                }

                if (this.OUT_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.OUT_TIME.Value <= this.OUT_TIME_TO.Value);
                }

                if (!String.IsNullOrEmpty(this.REQUEST_DEPARTMENT_CODE_EXACT))
                {
                    listExpression.Add(o => o.REQUEST_DEPARTMENT_CODE == this.REQUEST_DEPARTMENT_CODE_EXACT);
                }

                if (!String.IsNullOrEmpty(this.EXECUTE_DEPARTMENT_CODE_EXACT))
                {
                    listExpression.Add(o => o.EXECUTE_DEPARTMENT_CODE == this.EXECUTE_DEPARTMENT_CODE_EXACT);
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    string key = this.KEY_WORD.ToLower();
                    listExpression.Add(o => o.EXECUTE_ROOM_CODE.ToLower().Contains(key)
                        || o.EXECUTE_ROOM_NAME.ToLower().Contains(key)
                        || o.PATIENT_CODE.ToLower().Contains(key)
                        || o.PATIENT_NAME.ToLower().Contains(key)
                        || o.PATIENT_TYPE_CODE.ToLower().Contains(key)
                        || o.PATIENT_TYPE_NAME.ToLower().Contains(key)
                        || o.REQUEST_ROOM_CODE.ToLower().Contains(key)
                        || o.REQUEST_ROOM_NAME.ToLower().Contains(key)
                        || o.SERVICE_CODE.ToLower().Contains(key)
                        || o.SERVICE_NAME.ToLower().Contains(key)
                        || o.SERVICE_TYPE_CODE.ToLower().Contains(key)
                        || o.SERVICE_TYPE_NAME.ToLower().Contains(key)
                        || o.XANHPON_SYMBOL.ToLower().Contains(key));
                }

                search.listHtcRevenueExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<HTC_REVENUE>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHtcRevenueExpression.Clear();
                search.listHtcRevenueExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
