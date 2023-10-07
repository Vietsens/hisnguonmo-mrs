using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndex
{
    public class HisTestIndexViewFilterQuery : HisTestIndexViewFilter
    {
        public HisTestIndexViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_TEST_INDEX, bool>>> listVHisTestIndexExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TEST_INDEX, bool>>>();

        

        internal HisTestIndexSO Query()
        {
            HisTestIndexSO search = new HisTestIndexSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisTestIndexExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisTestIndexExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisTestIndexExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisTestIndexExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisTestIndexExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_IDs != null)
                {
                    search.listVHisTestIndexExpression.Add(o => this.SERVICE_IDs.Contains(o.TEST_SERVICE_TYPE_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisTestIndexExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisTestIndexExpression.AddRange(listVHisTestIndexExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisTestIndexExpression.Clear();
                search.listVHisTestIndexExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
