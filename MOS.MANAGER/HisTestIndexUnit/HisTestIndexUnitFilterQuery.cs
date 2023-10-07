using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexUnit
{
    public class HisTestIndexUnitFilterQuery : HisTestIndexUnitFilter
    {
        public HisTestIndexUnitFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_UNIT, bool>>> listHisTestIndexUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_UNIT, bool>>>();

        

        internal HisTestIndexUnitSO Query()
        {
            HisTestIndexUnitSO search = new HisTestIndexUnitSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTestIndexUnitExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisTestIndexUnitExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisTestIndexUnitExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisTestIndexUnitExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisTestIndexUnitExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisTestIndexUnitExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TEST_INDEX_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisTestIndexUnitExpression.AddRange(listHisTestIndexUnitExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTestIndexUnitExpression.Clear();
                search.listHisTestIndexUnitExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
