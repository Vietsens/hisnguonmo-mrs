using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    public class HisTransactionFilterQuery : HisTransactionFilter
    {
        public HisTransactionFilterQuery()
            : base()
        {

        }


        internal List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION, bool>>> listHisTransactionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TRANSACTION, bool>>>();



        internal HisTransactionSO Query()
        {
            HisTransactionSO search = new HisTransactionSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTransactionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisTransactionExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisTransactionExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisTransactionExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TRANSACTION_TYPE_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.TRANSACTION_TYPE_ID == this.TRANSACTION_TYPE_ID.Value);
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.ACCOUNT_BOOK_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.ACCOUNT_BOOK_ID == this.ACCOUNT_BOOK_ID.Value);
                }
                if (this.PAY_FORM_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.PAY_FORM_ID == this.PAY_FORM_ID.Value);
                }
                if (this.PAY_FORM_IDs != null)
                {
                    search.listHisTransactionExpression.Add(o => this.PAY_FORM_IDs.Contains(o.PAY_FORM_ID));
                }
                if (this.CASHIER_ROOM_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CASHIER_ROOM_ID == this.CASHIER_ROOM_ID.Value);
                }
                if (this.CASHIER_ROOM_IDs != null)
                {
                    search.listHisTransactionExpression.Add(o => this.CASHIER_ROOM_IDs.Contains(o.CASHIER_ROOM_ID));
                }
                if (this.CASHOUT_ID.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue && o.CASHOUT_ID.Value == this.CASHOUT_ID.Value);
                }
                if (this.CASHOUT_IDs != null)
                {
                    search.listHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue && this.CASHOUT_IDs.Contains(o.CASHOUT_ID.Value));
                }
                if (this.NUM_ORDER_FROM.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.NUM_ORDER >= this.NUM_ORDER_FROM.Value);
                }
                if (this.NUM_ORDER_TO.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.NUM_ORDER <= this.NUM_ORDER_TO.Value);
                }
                if (this.TRANSACTION_TIME_FROM.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.TRANSACTION_TIME >= this.TRANSACTION_TIME_FROM.Value);
                }
                if (this.TRANSACTION_TIME_TO.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.TRANSACTION_TIME <= this.TRANSACTION_TIME_TO.Value);
                }
                if (this.CANCEL_TIME_FROM.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CANCEL_TIME >= this.CANCEL_TIME_FROM.Value);
                }
                if (this.CANCEL_TIME_TO.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.CANCEL_TIME <= this.CANCEL_TIME_TO.Value);
                }
                if (this.HAS_CASHOUT.HasValue && this.HAS_CASHOUT.Value)
                {
                    search.listHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue);
                }
                if (this.HAS_CASHOUT.HasValue && !this.HAS_CASHOUT.Value)
                {
                    search.listHisTransactionExpression.Add(o => !o.CASHOUT_ID.HasValue);
                }
                if (this.NUM_ORDER__EQUAL.HasValue)
                {
                    search.listHisTransactionExpression.Add(o => o.NUM_ORDER == this.NUM_ORDER__EQUAL.Value);
                }
                if (this.HAS_TDL_SERE_SERV_DEPOSIT.HasValue)
                {
                    if (this.HAS_TDL_SERE_SERV_DEPOSIT.Value)
                    {
                        listHisTransactionExpression.Add(o => o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && o.TDL_SERE_SERV_DEPOSIT_COUNT.Value > 0);
                    }
                    else
                    {
                        listHisTransactionExpression.Add(o => !o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue || o.TDL_SERE_SERV_DEPOSIT_COUNT.Value == 0);
                    }
                }
                if (this.HAS_TDL_SESE_DEPO_REPAY.HasValue)
                {
                    if (this.HAS_TDL_SESE_DEPO_REPAY.Value)
                    {
                        listHisTransactionExpression.Add(o => o.TDL_SESE_DEPO_REPAY_COUNT.HasValue && o.TDL_SESE_DEPO_REPAY_COUNT.Value > 0);
                    }
                    else
                    {
                        listHisTransactionExpression.Add(o => !o.TDL_SESE_DEPO_REPAY_COUNT.HasValue || o.TDL_SESE_DEPO_REPAY_COUNT.Value == 0);
                    }
                }
                if (this.TREATMENT_IDs != null)
                {
                    search.listHisTransactionExpression.Add(o => o.TREATMENT_ID.HasValue && this.TREATMENT_IDs.Contains(o.TREATMENT_ID.Value));
                }
                if (this.IS_CANCEL.HasValue)
                {
                    if (this.IS_CANCEL.Value)
                    {
                        listHisTransactionExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listHisTransactionExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.HAS_SALL_TYPE.HasValue)
                {
                    if (this.HAS_SALL_TYPE.Value)
                    {
                        listHisTransactionExpression.Add(o => o.SALE_TYPE_ID.HasValue);
                    }
                    else
                    {
                        listHisTransactionExpression.Add(o => !o.SALE_TYPE_ID.HasValue);
                    }
                }

                search.listHisTransactionExpression.AddRange(listHisTransactionExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTransactionExpression.Clear();
                search.listHisTransactionExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
