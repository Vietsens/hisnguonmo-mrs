using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAcinInteractive
{
    public class HisAcinInteractiveViewFilterQuery : HisAcinInteractiveViewFilter
    {
        public HisAcinInteractiveViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ACIN_INTERACTIVE, bool>>> listVHisAcinInteractiveExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACIN_INTERACTIVE, bool>>>();

        

        internal HisAcinInteractiveSO Query()
        {
            HisAcinInteractiveSO search = new HisAcinInteractiveSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisAcinInteractiveExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisAcinInteractiveExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisAcinInteractiveExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisAcinInteractiveExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisAcinInteractiveExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAcinInteractiveExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGREDIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGREDIENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.CONFLICT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.CONFLICT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisAcinInteractiveExpression.AddRange(listVHisAcinInteractiveExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisAcinInteractiveExpression.Clear();
                search.listVHisAcinInteractiveExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
