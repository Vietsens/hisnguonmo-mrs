using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using SAR.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportTemplate.Get
{
    public class SarReportTemplateViewFilterQuery : SarReportTemplateViewFilter
    {
        public SarReportTemplateViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT_TEMPLATE, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<V_SAR_REPORT_TEMPLATE, bool>>>();

        internal OrderProcessorBase OrderProcess = new OrderProcessorBase();

        internal SarReportTemplateSO Query()
        {
            SarReportTemplateSO search = new SarReportTemplateSO();
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

                if (!String.IsNullOrEmpty(this.REPORT_TEMPLATE_CODE))
                {
                    listExpression.Add(o => o.REPORT_TEMPLATE_CODE == this.REPORT_TEMPLATE_CODE);
                }
                if (!String.IsNullOrEmpty(this.REPORT_TYPE_CODE))
                {
                    listExpression.Add(o => o.REPORT_TYPE_CODE == this.REPORT_TYPE_CODE);
                }
                if (this.REPORT_TYPE_ID.HasValue)
                {
                    listExpression.Add(o => o.REPORT_TYPE_ID == this.REPORT_TYPE_ID.Value);
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EXTENSION_RECEIVE.ToLower().Contains(this.KEY_WORD) ||
                        o.TUTORIAL.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TEMPLATE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TEMPLATE_NAME.ToLower().Contains(this.KEY_WORD));
                }
                #endregion

                search.listVSarReportTemplateExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<V_SAR_REPORT_TEMPLATE>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVSarReportTemplateExpression.Clear();
                search.listVSarReportTemplateExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
