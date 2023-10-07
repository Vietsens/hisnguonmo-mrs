using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndex
{
    public class HisTestIndexFilterQuery : HisTestIndexFilter
    {
        public HisTestIndexFilterQuery()
            : base()
        {

        }


        internal List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX, bool>>> listHisTestIndexExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX, bool>>>();

        

        internal HisTestIndexSO Query()
        {
            HisTestIndexSO search = new HisTestIndexSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTestIndexExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisTestIndexExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisTestIndexExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisTestIndexExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisTestIndexExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.TEST_INDEX_UNIT_ID.HasValue)
                {
                    search.listHisTestIndexExpression.Add(o => o.TEST_INDEX_UNIT_ID.HasValue && o.TEST_INDEX_UNIT_ID.Value == this.TEST_INDEX_UNIT_ID.Value);
                }

                search.listHisTestIndexExpression.AddRange(listHisTestIndexExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTestIndexExpression.Clear();
                search.listHisTestIndexExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
