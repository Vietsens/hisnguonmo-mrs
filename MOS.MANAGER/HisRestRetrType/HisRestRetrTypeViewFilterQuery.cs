using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRestRetrType
{
    public class HisRestRetrTypeViewFilterQuery : HisRestRetrTypeViewFilter
    {
        public HisRestRetrTypeViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_REST_RETR_TYPE, bool>>> listVHisRestRetrTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REST_RETR_TYPE, bool>>>();

        

        internal HisRestRetrTypeSO Query()
        {
            HisRestRetrTypeSO search = new HisRestRetrTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisRestRetrTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisRestRetrTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisRestRetrTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisRestRetrTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.REHA_TRAIN_UNIT_ID.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.REHA_TRAIN_UNIT_ID == this.REHA_TRAIN_UNIT_ID.Value);
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.REHA_SERVICE_TYPE_ID == this.SERVICE_ID.Value);
                }
                if (this.REHA_TRAIN_TYPE_ID.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.REHA_TRAIN_TYPE_ID == this.REHA_TRAIN_TYPE_ID.Value);
                }
                if (this.REHA_SERVICE_TYPE_ID.HasValue)
                {
                    listVHisRestRetrTypeExpression.Add(o => o.REHA_SERVICE_TYPE_ID == this.REHA_SERVICE_TYPE_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisRestRetrTypeExpression.Add(o =>
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisRestRetrTypeExpression.AddRange(listVHisRestRetrTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisRestRetrTypeExpression.Clear();
                search.listVHisRestRetrTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
