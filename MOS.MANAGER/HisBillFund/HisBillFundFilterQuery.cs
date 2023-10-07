using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    public class HisBillFundFilterQuery : HisBillFundFilter
    {
        public HisBillFundFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_BILL_FUND, bool>>> listHisBillFundExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BILL_FUND, bool>>>();

        

        internal HisBillFundSO Query()
        {
            HisBillFundSO search = new HisBillFundSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisBillFundExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisBillFundExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisBillFundExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisBillFundExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisBillFundExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BILL_ID.HasValue)
                {
                    listHisBillFundExpression.Add(o => o.BILL_ID == this.BILL_ID.Value);
                }
                if (this.BILL_IDs != null)
                {
                    listHisBillFundExpression.Add(o => this.BILL_IDs.Contains(o.BILL_ID));
                }

                search.listHisBillFundExpression.AddRange(listHisBillFundExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisBillFundExpression.Clear();
                search.listHisBillFundExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
