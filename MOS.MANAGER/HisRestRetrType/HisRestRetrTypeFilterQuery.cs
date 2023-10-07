using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRestRetrType
{
    public class HisRestRetrTypeFilterQuery : HisRestRetrTypeFilter
    {
        public HisRestRetrTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_REST_RETR_TYPE, bool>>> listHisRestRetrTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REST_RETR_TYPE, bool>>>();

        

        internal HisRestRetrTypeSO Query()
        {
            HisRestRetrTypeSO search = new HisRestRetrTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisRestRetrTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisRestRetrTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisRestRetrTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisRestRetrTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.REHA_TRAIN_TYPE_ID.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.REHA_TRAIN_TYPE_ID == this.REHA_TRAIN_TYPE_ID.Value);
                }
                if (this.REHA_SERVICE_TYPE_ID.HasValue)
                {
                    listHisRestRetrTypeExpression.Add(o => o.REHA_SERVICE_TYPE_ID == this.REHA_SERVICE_TYPE_ID.Value);
                }
                
                search.listHisRestRetrTypeExpression.AddRange(listHisRestRetrTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisRestRetrTypeExpression.Clear();
                search.listHisRestRetrTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
