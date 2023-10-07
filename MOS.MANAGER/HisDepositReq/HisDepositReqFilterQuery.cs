using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepositReq
{
    public class HisDepositReqFilterQuery : HisDepositReqFilter
    {
        public HisDepositReqFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_DEPOSIT_REQ, bool>>> listHisDepositReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEPOSIT_REQ, bool>>>();

        

        internal HisDepositReqSO Query()
        {
            HisDepositReqSO search = new HisDepositReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisDepositReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisDepositReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisDepositReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisDepositReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisDepositReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listHisDepositReqExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisDepositReqExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.HAS_DEPOSIT.HasValue && this.HAS_DEPOSIT.Value)
                {
                    listHisDepositReqExpression.Add(o => o.DEPOSIT_ID.HasValue);
                }
                if (this.HAS_DEPOSIT.HasValue && !this.HAS_DEPOSIT.Value)
                {
                    listHisDepositReqExpression.Add(o => !o.DEPOSIT_ID.HasValue);
                }
                if (!String.IsNullOrEmpty(this.DEPOSIT_REQ_CODE__EXACT))
                {
                    listHisDepositReqExpression.Add(o => o.DEPOSIT_REQ_CODE == this.DEPOSIT_REQ_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisDepositReqExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPOSIT_REQ_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_USERNAME.ToLower().Contains(this.KEY_WORD));
                }

                search.listHisDepositReqExpression.AddRange(listHisDepositReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisDepositReqExpression.Clear();
                search.listHisDepositReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
