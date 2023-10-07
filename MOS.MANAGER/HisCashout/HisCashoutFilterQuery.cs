using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashout
{
    public class HisCashoutFilterQuery : HisCashoutFilter
    {
        public HisCashoutFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_CASHOUT, bool>>> listHisCashoutExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CASHOUT, bool>>>();

        

        internal HisCashoutSO Query()
        {
            HisCashoutSO search = new HisCashoutSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisCashoutExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisCashoutExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisCashoutExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisCashoutExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisCashoutExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.CASHOUT_TIME_TO.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.CASHOUT_TIME <= this.CASHOUT_TIME_TO.Value);
                }
                if (this.CASHOUT_TIME_FROM.HasValue)
                {
                    listHisCashoutExpression.Add(o => o.CASHOUT_TIME >= this.CASHOUT_TIME_FROM.Value);
                }

                search.listHisCashoutExpression.AddRange(listHisCashoutExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisCashoutExpression.Clear();
                search.listHisCashoutExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
