using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTracking
{
    public class HisTrackingFilterQuery : HisTrackingFilter
    {
        public HisTrackingFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TRACKING, bool>>> listHisTrackingExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TRACKING, bool>>>();

        

        internal HisTrackingSO Query()
        {
            HisTrackingSO search = new HisTrackingSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisTrackingExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisTrackingExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisTrackingExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisTrackingExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisTrackingExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.TREATMENT_IDs != null)
                {
                    listHisTrackingExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TRACKING_TIME_FROM.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.TRACKING_TIME >= this.TRACKING_TIME_FROM.Value);
                }
                if (this.TRACKING_TIME_TO.HasValue)
                {
                    listHisTrackingExpression.Add(o => o.TRACKING_TIME <= this.TRACKING_TIME_TO.Value);
                }

                search.listHisTrackingExpression.AddRange(listHisTrackingExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTrackingExpression.Clear();
                search.listHisTrackingExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
