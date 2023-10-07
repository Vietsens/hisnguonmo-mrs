using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccountBook
{
    public class HisAccountBookFilterQuery : HisAccountBookFilter
    {
        public HisAccountBookFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ACCOUNT_BOOK, bool>>> listHisAccountBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCOUNT_BOOK, bool>>>();

        

        internal HisAccountBookSO Query()
        {
            HisAccountBookSO search = new HisAccountBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisAccountBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAccountBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAccountBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAccountBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAccountBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.FOR_BILL.HasValue)
                {
                    if (this.FOR_BILL.Value)
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_BILL.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_BILL.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.FOR_DEPOSIT.HasValue)
                {
                    if (this.FOR_DEPOSIT.Value)
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_DEPOSIT.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_DEPOSIT.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.FOR_REPAY.HasValue)
                {
                    if (this.FOR_REPAY.Value)
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_REPAY.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listHisAccountBookExpression.Add(o => o.IS_FOR_REPAY.Value != ManagerConstant.IS_TRUE);
                    }
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisAccountBookExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCOUNT_BOOK_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCOUNT_BOOK_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisAccountBookExpression.AddRange(listHisAccountBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                listHisAccountBookExpression.Clear();
                search.listHisAccountBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
