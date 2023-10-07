using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndex
{
    public class HisSuimIndexViewFilterQuery : HisSuimIndexViewFilter
    {
        public HisSuimIndexViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_INDEX, bool>>> listVHisSuimIndexExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_INDEX, bool>>>();

        

        internal HisSuimIndexSO Query()
        {
            HisSuimIndexSO search = new HisSuimIndexSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisSuimIndexExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisSuimIndexExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisSuimIndexExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisSuimIndexExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisSuimIndexExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisSuimIndexExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUIM_INDEX_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisSuimIndexExpression.AddRange(listVHisSuimIndexExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSuimIndexExpression.Clear();
                search.listVHisSuimIndexExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
