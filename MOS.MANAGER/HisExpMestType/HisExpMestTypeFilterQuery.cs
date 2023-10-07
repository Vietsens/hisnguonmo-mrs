using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestType
{
    public class HisExpMestTypeFilterQuery : HisExpMestTypeFilter
    {
        public HisExpMestTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_TYPE, bool>>> listHisExpMestTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_TYPE, bool>>>();

        

        internal HisExpMestTypeSO Query()
        {
            HisExpMestTypeSO search = new HisExpMestTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisExpMestTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisExpMestTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisExpMestTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisExpMestTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisExpMestTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (!String.IsNullOrEmpty(this.EXP_MEST_TYPE_CODE))
                {
                    this.EXP_MEST_TYPE_CODE = this.EXP_MEST_TYPE_CODE.Trim().ToLower();
                    search.listHisExpMestTypeExpression.Add(o => o.EXP_MEST_TYPE_CODE.ToLower().Contains(this.EXP_MEST_TYPE_CODE));
                }
                #endregion

                search.listHisExpMestTypeExpression.AddRange(listHisExpMestTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestTypeExpression.Clear();
                search.listHisExpMestTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
