using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransactionType
{
    public class HisTransactionTypeFilterQuery : HisTransactionTypeFilter
    {
        public HisTransactionTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION_TYPE, bool>>> listHisTransactionTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION_TYPE, bool>>>();

        

        internal HisTransactionTypeSO Query()
        {
            HisTransactionTypeSO search = new HisTransactionTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTransactionTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisTransactionTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisTransactionTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisTransactionTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisTransactionTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.TRANSACTION_TYPE_CODE))
                {
                    this.TRANSACTION_TYPE_CODE = this.TRANSACTION_TYPE_CODE.Trim().ToLower();
                    search.listHisTransactionTypeExpression.Add(o => o.TRANSACTION_TYPE_CODE.ToLower().Contains(this.TRANSACTION_TYPE_CODE));
                }
                
                search.listHisTransactionTypeExpression.AddRange(listHisTransactionTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTransactionTypeExpression.Clear();
                search.listHisTransactionTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
