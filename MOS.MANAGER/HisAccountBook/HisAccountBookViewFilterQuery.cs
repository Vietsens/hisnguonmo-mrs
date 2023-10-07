using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccountBook
{
    public class HisAccountBookViewFilterQuery : HisAccountBookViewFilter
    {
        public HisAccountBookViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ACCOUNT_BOOK, bool>>> listVHisAccountBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACCOUNT_BOOK, bool>>>();

        internal HisAccountBookSO Query()
        {
            HisAccountBookSO search = new HisAccountBookSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisAccountBookExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisAccountBookExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisAccountBookExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisAccountBookExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisAccountBookExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.ACCOUNT_BOOK_NAME))
                {
                    listVHisAccountBookExpression.Add(o => o.ACCOUNT_BOOK_NAME != null && o.ACCOUNT_BOOK_NAME.ToLower().Contains(this.ACCOUNT_BOOK_NAME.ToLower()));
                }
                if (!String.IsNullOrEmpty(this.ACCOUNT_BOOK_CODE))
                {
                    listVHisAccountBookExpression.Add(o => o.ACCOUNT_BOOK_CODE == this.ACCOUNT_BOOK_CODE);
                }
                if (this.IS_OUT_OF_BILL.HasValue)
                {
                    if (this.IS_OUT_OF_BILL.Value)
                    {
                        listVHisAccountBookExpression.Add(o => o.CURRENT_NUM_ORDER >= (o.FROM_NUM_ORDER + o.TOTAL - 1));
                    }
                    else
                    {
                        listVHisAccountBookExpression.Add(o => o.CURRENT_NUM_ORDER < (o.FROM_NUM_ORDER + o.TOTAL - 1));
                    }
                }
                if (this.FOR_BILL.HasValue)
                {
                    if (this.FOR_BILL.Value)
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_BILL.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_BILL.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.FOR_DEPOSIT.HasValue)
                {
                    if (this.FOR_DEPOSIT.Value)
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_DEPOSIT.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_DEPOSIT.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.FOR_REPAY.HasValue)
                {
                    if (this.FOR_REPAY.Value)
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_REPAY.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listVHisAccountBookExpression.Add(o => o.IS_FOR_REPAY.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAccountBookExpression.Add(o => o.ACCOUNT_BOOK_CODE.ToLower().Contains(this.KEY_WORD) || o.ACCOUNT_BOOK_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (this.TRANSACTION_TYPE_IDs != null)
                {
                    listVHisAccountBookExpression.Add(o =>
                        ((!this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) && o.IS_FOR_BILL.Value != ManagerConstant.IS_TRUE) || (this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) && o.IS_FOR_BILL.Value == ManagerConstant.IS_TRUE))
                        ||
                        ((!this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) && o.IS_FOR_REPAY.Value != ManagerConstant.IS_TRUE) || (this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) && o.IS_FOR_REPAY.Value == ManagerConstant.IS_TRUE))
                        ||
                        ((!this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU) && o.IS_FOR_DEPOSIT.Value != ManagerConstant.IS_TRUE) || (this.TRANSACTION_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU) && o.IS_FOR_DEPOSIT.Value == ManagerConstant.IS_TRUE))
                        );
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAccountBookExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCOUNT_BOOK_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCOUNT_BOOK_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisAccountBookExpression.AddRange(listVHisAccountBookExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisAccountBookExpression.Clear();
                search.listVHisAccountBookExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
