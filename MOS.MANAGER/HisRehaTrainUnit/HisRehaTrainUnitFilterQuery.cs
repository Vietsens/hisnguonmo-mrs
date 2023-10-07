using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainUnit
{
    public class HisRehaTrainUnitFilterQuery : HisRehaTrainUnitFilter
    {
        public HisRehaTrainUnitFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_UNIT, bool>>> listHisRehaTrainUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_UNIT, bool>>>();

        

        internal HisRehaTrainUnitSO Query()
        {
            HisRehaTrainUnitSO search = new HisRehaTrainUnitSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisRehaTrainUnitExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisRehaTrainUnitExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisRehaTrainUnitExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisRehaTrainUnitExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisRehaTrainUnitExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisRehaTrainUnitExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REHA_TRAIN_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                search.listHisRehaTrainUnitExpression.AddRange(listHisRehaTrainUnitExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisRehaTrainUnitExpression.Clear();
                search.listHisRehaTrainUnitExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
