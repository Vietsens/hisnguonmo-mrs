using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisActiveIngredient
{
    public class HisActiveIngredientFilterQuery : HisActiveIngredientFilter
    {
        public HisActiveIngredientFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ACTIVE_INGREDIENT, bool>>> listHisActiveIngredientExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACTIVE_INGREDIENT, bool>>>();

        

        internal HisActiveIngredientSO Query()
        {
            HisActiveIngredientSO search = new HisActiveIngredientSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisActiveIngredientExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisActiveIngredientExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisActiveIngredientExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisActiveIngredientExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisActiveIngredientExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisActiveIngredientExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGREDIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGREDIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (!String.IsNullOrEmpty(this.CN_WORD))
                {
                    this.CN_WORD = this.CN_WORD.ToLower().Trim();
                    search.listHisActiveIngredientExpression.Add(o => o.ACTIVE_INGREDIENT_CODE.ToLower().Contains(this.CN_WORD) || o.ACTIVE_INGREDIENT_NAME.ToLower().Contains(this.CN_WORD));
                }
                
                search.listHisActiveIngredientExpression.AddRange(listHisActiveIngredientExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisActiveIngredientExpression.Clear();
                search.listHisActiveIngredientExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
