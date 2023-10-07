using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using SAR.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportCalendar.Get
{
    public class SarReportCalendarFilterQuery : SarReportCalendarFilter
    {
        public SarReportCalendarFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<SAR_REPORT_CALENDAR, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<SAR_REPORT_CALENDAR, bool>>>();

        internal OrderProcessorBase OrderProcess = new OrderProcessorBase();

        internal SarReportCalendarSO Query()
        {
            SarReportCalendarSO search = new SarReportCalendarSO();
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
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
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

                if (this.EXECUTE.HasValue)
                {
                    switch (this.EXECUTE)
                    {
                        case ExecuteEnum.EXECUTED:
                            listExpression.Add(o => o.IS_DONE == 1);
                            break;
                        case ExecuteEnum.NOT_EXECUTE:
                            listExpression.Add(o => o.IS_DONE == null || o.IS_DONE != 1);
                            break;
                        default:
                            break;
                    }
                }
                if (this.SCAN_TIME.HasValue)
                {
                    listExpression.Add(o => o.EXECUTE_TIME <= this.SCAN_TIME.Value);
                }
                #endregion

                search.listSarReportCalendarExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<SAR_REPORT_CALENDAR>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listSarReportCalendarExpression.Clear();
                search.listSarReportCalendarExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
