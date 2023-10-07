using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainType
{
    public class HisRehaTrainTypeViewFilterQuery : HisRehaTrainTypeViewFilter
    {
        public HisRehaTrainTypeViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN_TYPE, bool>>> listVHisRehaTrainTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN_TYPE, bool>>>();

        

        internal HisRehaTrainTypeSO Query()
        {
            HisRehaTrainTypeSO search = new HisRehaTrainTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisRehaTrainTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisRehaTrainTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.REHA_TRAIN_UNIT_ID.HasValue)
                {
                    listVHisRehaTrainTypeExpression.Add(o => o.REHA_TRAIN_UNIT_ID == this.REHA_TRAIN_UNIT_ID.Value);
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisRehaTrainTypeExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD)
                        || o.REHA_TRAIN_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REHA_TRAIN_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.MODIFIER.ToLower().Contains(this.KEY_WORD)
                        || o.REHA_TRAIN_UNIT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REHA_TRAIN_UNIT_NAME.ToLower().Contains(this.KEY_WORD));
                }

                search.listVHisRehaTrainTypeExpression.AddRange(listVHisRehaTrainTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisRehaTrainTypeExpression.Clear();
                search.listVHisRehaTrainTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
