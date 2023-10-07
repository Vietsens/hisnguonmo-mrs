using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReportTypeCat
{
    public class HisReportTypeCatFilterQuery : HisReportTypeCatFilter
    {
        public HisReportTypeCatFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_REPORT_TYPE_CAT, bool>>> listHisReportTypeCatExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REPORT_TYPE_CAT, bool>>>();

        

        internal HisReportTypeCatSO Query()
        {
            HisReportTypeCatSO search = new HisReportTypeCatSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisReportTypeCatExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisReportTypeCatExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisReportTypeCatExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisReportTypeCatExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisReportTypeCatExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.REPORT_TYPE_CODE__EXACT))
                {
                    listHisReportTypeCatExpression.Add(o => o.REPORT_TYPE_CODE != null && o.REPORT_TYPE_CODE == this.REPORT_TYPE_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.CATEGORY_CODE__EXACT))
                {
                    listHisReportTypeCatExpression.Add(o => o.CATEGORY_CODE != null && o.CATEGORY_CODE == this.CATEGORY_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.CATEGORY_NAME))
                {
                    this.CATEGORY_NAME = this.CATEGORY_NAME.ToLower();
                    listHisReportTypeCatExpression.Add(o => o.CATEGORY_NAME != null && o.CATEGORY_NAME.Contains(this.CATEGORY_NAME));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisReportTypeCatExpression.Add(o =>
                        o.CATEGORY_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.CATEGORY_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisReportTypeCatExpression.AddRange(listHisReportTypeCatExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisReportTypeCatExpression.Clear();
                search.listHisReportTypeCatExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
