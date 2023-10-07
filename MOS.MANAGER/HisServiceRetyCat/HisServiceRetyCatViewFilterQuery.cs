using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRetyCat
{
    public class HisServiceRetyCatViewFilterQuery : HisServiceRetyCatViewFilter
    {
        public HisServiceRetyCatViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_RETY_CAT, bool>>> listVHisServiceRetyCatExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_RETY_CAT, bool>>>();

        

        internal HisServiceRetyCatSO Query()
        {
            HisServiceRetyCatSO search = new HisServiceRetyCatSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisServiceRetyCatExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisServiceRetyCatExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisServiceRetyCatExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisServiceRetyCatExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.REPORT_TYPE_CAT_ID.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.REPORT_TYPE_CAT_ID == this.REPORT_TYPE_CAT_ID.Value);
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listVHisServiceRetyCatExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.REPORT_TYPE_CODE__EXACT))
                {
                    listVHisServiceRetyCatExpression.Add(o => o.REPORT_TYPE_CODE != null && o.REPORT_TYPE_CODE == this.REPORT_TYPE_CODE__EXACT);
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisServiceRetyCatExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.REPORT_TYPE_CAT_IDs != null)
                {
                    listVHisServiceRetyCatExpression.Add(o => this.REPORT_TYPE_CAT_IDs.Contains(o.REPORT_TYPE_CAT_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisServiceRetyCatExpression.Add(o =>
                        o.CATEGORY_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.CATEGORY_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisServiceRetyCatExpression.AddRange(listVHisServiceRetyCatExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisServiceRetyCatExpression.Clear();
                search.listVHisServiceRetyCatExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
