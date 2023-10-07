using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    public class HisTransactionViewFilterQuery : HisTransactionViewFilter
    {
        public HisTransactionViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION, bool>>> listVHisTransactionExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TRANSACTION, bool>>>();



        internal HisTransactionSO Query()
        {
            HisTransactionSO search = new HisTransactionSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisTransactionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisTransactionExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisTransactionExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisTransactionExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.ACCOUNT_BOOK_ID.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.ACCOUNT_BOOK_ID == this.ACCOUNT_BOOK_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    search.listVHisTransactionExpression.Add(o => o.TRANSACTION_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.ACCOUNT_BOOK_CODE.ToLower().Contains(this.KEY_WORD) 
                        || o.ACCOUNT_BOOK_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD) 
                        || o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD) 
                        || o.TRANSACTION_TYPE_NAME.ToLower().Contains(this.KEY_WORD) 
                        || o.PAY_FORM_NAME.ToLower().Contains(this.KEY_WORD) 
                        || o.TREATMENT_CODE.ToLower().Contains(this.KEY_WORD));
                }
                if (this.TRANSACTION_TYPE_IDs != null)
                {
                    search.listVHisTransactionExpression.Add(o => this.TRANSACTION_TYPE_IDs.Contains(o.TRANSACTION_TYPE_ID));
                }
                if (this.TREATMENT_IDs != null)
                {
                    search.listVHisTransactionExpression.Add(o => o.TREATMENT_ID.HasValue && this.TREATMENT_IDs.Contains(o.TREATMENT_ID.Value));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.TREATMENT_ID.HasValue && o.TREATMENT_ID.Value == this.TREATMENT_ID.Value);
                }
                if (this.IS_CANCEL.HasValue && this.IS_CANCEL.Value)
                {
                    search.listVHisTransactionExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CANCEL.HasValue && !this.IS_CANCEL.Value)
                {
                    search.listVHisTransactionExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL.Value != ManagerConstant.IS_TRUE);
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE__EXACT))
                {
                    search.listVHisTransactionExpression.Add(o => o.TREATMENT_CODE == this.TREATMENT_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.ACCOUNT_BOOK_CODE__EXACT))
                {
                    search.listVHisTransactionExpression.Add(o => o.ACCOUNT_BOOK_CODE == this.ACCOUNT_BOOK_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TRANSACTION_CODE__EXACT))
                {
                    listVHisTransactionExpression.Add(o => o.TRANSACTION_CODE == this.TRANSACTION_CODE__EXACT);
                }
                if (this.CASHOUT_IDs != null)
                {
                    search.listVHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue && this.CASHOUT_IDs.Contains(o.CASHOUT_ID.Value));
                }
                if (this.CASHOUT_ID.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue && o.CASHOUT_ID.Value == this.CASHOUT_ID.Value);
                }
                if (this.ACCOUNT_BOOK_IDs != null)
                {
                    search.listVHisTransactionExpression.Add(o => this.ACCOUNT_BOOK_IDs.Contains(o.ACCOUNT_BOOK_ID));
                }
                if (this.NUM_ORDER_FROM.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.NUM_ORDER >= this.NUM_ORDER_FROM.Value);
                }
                if (this.NUM_ORDER_TO.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.NUM_ORDER <= this.NUM_ORDER_TO.Value);
                }
                if (this.NUM_ORDER__EQUAL.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.NUM_ORDER == this.NUM_ORDER__EQUAL.Value);
                }
                if (this.TRANSACTION_TIME_FROM.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.TRANSACTION_TIME >= this.TRANSACTION_TIME_FROM.Value);
                }
                if (this.TRANSACTION_TIME_TO.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.TRANSACTION_TIME <= this.TRANSACTION_TIME_TO.Value);
                }
                if (this.HAS_CASHOUT.HasValue && this.HAS_CASHOUT.Value)
                {
                    search.listVHisTransactionExpression.Add(o => o.CASHOUT_ID.HasValue);
                }
                if (this.HAS_CASHOUT.HasValue && !this.HAS_CASHOUT.Value)
                {
                    search.listVHisTransactionExpression.Add(o => !o.CASHOUT_ID.HasValue);
                }
                if (this.CANCEL_TIME_FROM.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.CANCEL_TIME >= this.CANCEL_TIME_FROM.Value);
                }
                if (this.CANCEL_TIME_TO.HasValue)
                {
                    search.listVHisTransactionExpression.Add(o => o.CANCEL_TIME <= this.CANCEL_TIME_TO.Value);
                }
                if (this.HAS_SALL_TYPE.HasValue)
                {
                    if (this.HAS_SALL_TYPE.Value)
                    {
                        search.listVHisTransactionExpression.Add(o => o.SALE_TYPE_ID.HasValue);
                    }
                    else
                    {
                        search.listVHisTransactionExpression.Add(o => !o.SALE_TYPE_ID.HasValue);
                    }
                }

                search.listVHisTransactionExpression.AddRange(listVHisTransactionExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisTransactionExpression.Clear();
                search.listVHisTransactionExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
