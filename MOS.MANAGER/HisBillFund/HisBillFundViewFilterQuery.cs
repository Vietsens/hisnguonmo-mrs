using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    public class HisBillFundViewFilterQuery : HisBillFundViewFilter
    {
        public HisBillFundViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BILL_FUND, bool>>> listVHisBillFundExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BILL_FUND, bool>>>();

        

        internal HisBillFundSO Query()
        {
            HisBillFundSO search = new HisBillFundSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisBillFundExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBillFundExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBillFundExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBillFundExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBillFundExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BILL_ID.HasValue)
                {
                    listVHisBillFundExpression.Add(o => o.BILL_ID == this.BILL_ID.Value);
                }
                if (this.BILL_IDs != null)
                {
                    listVHisBillFundExpression.Add(o => this.BILL_IDs.Contains(o.BILL_ID));
                }

                search.listVHisBillFundExpression.AddRange(listVHisBillFundExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBillFundExpression.Clear();
                search.listVHisBillFundExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
