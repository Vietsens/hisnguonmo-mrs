using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndexUnit
{
    public class HisSuimIndexUnitFilterQuery : HisSuimIndexUnitFilter
    {
        public HisSuimIndexUnitFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX_UNIT, bool>>> listHisSuimIndexUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX_UNIT, bool>>>();

        

        internal HisSuimIndexUnitSO Query()
        {
            HisSuimIndexUnitSO search = new HisSuimIndexUnitSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisSuimIndexUnitExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSuimIndexUnitExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSuimIndexUnitExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSuimIndexUnitExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSuimIndexUnitExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisSuimIndexUnitExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisSuimIndexUnitExpression.AddRange(listHisSuimIndexUnitExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSuimIndexUnitExpression.Clear();
                search.listHisSuimIndexUnitExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
