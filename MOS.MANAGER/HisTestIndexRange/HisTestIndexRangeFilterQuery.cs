using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexRange
{
    public class HisTestIndexRangeFilterQuery : HisTestIndexRangeFilter
    {
        public HisTestIndexRangeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_RANGE, bool>>> listHisTestIndexRangeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_RANGE, bool>>>();

        

        internal HisTestIndexRangeSO Query()
        {
            HisTestIndexRangeSO search = new HisTestIndexRangeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTestIndexRangeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TEST_INDEX_ID.HasValue)
                {
                    search.listHisTestIndexRangeExpression.Add(o => o.TEST_INDEX_ID == this.TEST_INDEX_ID.Value);
                }
                
                search.listHisTestIndexRangeExpression.AddRange(listHisTestIndexRangeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTestIndexRangeExpression.Clear();
                search.listHisTestIndexRangeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
