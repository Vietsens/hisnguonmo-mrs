using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockPeriod
{
    public class HisMediStockPeriodViewFilterQuery : HisMediStockPeriodViewFilter
    {
        public HisMediStockPeriodViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_PERIOD, bool>>> listVHisMediStockPeriodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK_PERIOD, bool>>>();

        

        internal HisMediStockPeriodSO Query()
        {
            HisMediStockPeriodSO search = new HisMediStockPeriodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    search.listVHisMediStockPeriodExpression.Add(o => o.MEDI_STOCK_PERIOD_NAME.ToLower().Contains(this.KEY_WORD));
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.PREVIOUS_ID.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.PREVIOUS_ID.HasValue && o.PREVIOUS_ID.Value == this.PREVIOUS_ID.Value);
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.ID));
                }
                if (this.TO_TIME_FROM.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.TO_TIME.Value >= this.TO_TIME_FROM.Value);
                }
                if (this.TO_TIME_TO.HasValue)
                {
                    search.listVHisMediStockPeriodExpression.Add(o => o.TO_TIME.Value <= this.TO_TIME_TO.Value);
                }

                search.listVHisMediStockPeriodExpression.AddRange(listVHisMediStockPeriodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMediStockPeriodExpression.Clear();
                search.listVHisMediStockPeriodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
