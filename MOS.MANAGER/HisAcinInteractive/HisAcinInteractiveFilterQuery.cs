using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAcinInteractive
{
    public class HisAcinInteractiveFilterQuery : HisAcinInteractiveFilter
    {
        public HisAcinInteractiveFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ACIN_INTERACTIVE, bool>>> listHisAcinInteractiveExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACIN_INTERACTIVE, bool>>>();

        

        internal HisAcinInteractiveSO Query()
        {
            HisAcinInteractiveSO search = new HisAcinInteractiveSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisAcinInteractiveExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAcinInteractiveExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAcinInteractiveExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAcinInteractiveExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.ACTIVE_INGREDIENT_ID__OR__ACTIVE_INGREDIENT_CONFLICT_ID.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.ACTIVE_INGREDIENT_ID == this.ACTIVE_INGREDIENT_ID__OR__ACTIVE_INGREDIENT_CONFLICT_ID.Value || o.CONFLICT_ID == this.ACTIVE_INGREDIENT_ID__OR__ACTIVE_INGREDIENT_CONFLICT_ID);
                }
                if (this.ACTIVE_INGREDIENT_ID.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.ACTIVE_INGREDIENT_ID == this.ACTIVE_INGREDIENT_ID.Value);
                }
                if (this.ACTIVE_INGREDIENT_CONFLICT_ID.HasValue)
                {
                    listHisAcinInteractiveExpression.Add(o => o.CONFLICT_ID == this.ACTIVE_INGREDIENT_CONFLICT_ID.Value);
                }
                search.listHisAcinInteractiveExpression.AddRange(listHisAcinInteractiveExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisAcinInteractiveExpression.Clear();
                search.listHisAcinInteractiveExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
