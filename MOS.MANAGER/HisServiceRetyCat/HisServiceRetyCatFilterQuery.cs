using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRetyCat
{
    public class HisServiceRetyCatFilterQuery : HisServiceRetyCatFilter
    {
        public HisServiceRetyCatFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_RETY_CAT, bool>>> listHisServiceRetyCatExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_RETY_CAT, bool>>>();



        internal HisServiceRetyCatSO Query()
        {
            HisServiceRetyCatSO search = new HisServiceRetyCatSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisServiceRetyCatExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisServiceRetyCatExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisServiceRetyCatExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisServiceRetyCatExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.REPORT_TYPE_CAT_ID.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.REPORT_TYPE_CAT_ID == this.REPORT_TYPE_CAT_ID.Value);
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listHisServiceRetyCatExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    listHisServiceRetyCatExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.REPORT_TYPE_CAT_IDs != null)
                {
                    listHisServiceRetyCatExpression.Add(o => this.REPORT_TYPE_CAT_IDs.Contains(o.REPORT_TYPE_CAT_ID));
                }

                search.listHisServiceRetyCatExpression.AddRange(listHisServiceRetyCatExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServiceRetyCatExpression.Clear();
                search.listHisServiceRetyCatExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
