using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndex
{
    public class HisSuimIndexFilterQuery : HisSuimIndexFilter
    {
        public HisSuimIndexFilterQuery()
            : base()
        {

        }


        internal List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX, bool>>> listHisSuimIndexExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_INDEX, bool>>>();

        

        internal HisSuimIndexSO Query()
        {
            HisSuimIndexSO search = new HisSuimIndexSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisSuimIndexExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisSuimIndexExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisSuimIndexExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisSuimIndexExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisSuimIndexExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.SUIM_INDEX_UNIT_ID.HasValue)
                {
                    search.listHisSuimIndexExpression.Add(o => o.SUIM_INDEX_UNIT_ID.HasValue && o.SUIM_INDEX_UNIT_ID.Value == this.SUIM_INDEX_UNIT_ID.Value);
                }

                search.listHisSuimIndexExpression.AddRange(listHisSuimIndexExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSuimIndexExpression.Clear();
                search.listHisSuimIndexExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
