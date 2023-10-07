using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMedi
{
    public class HisMestPeriodMediFilterQuery : HisMestPeriodMediFilter
    {
        public HisMestPeriodMediFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MEDI, bool>>> listHisMestPeriodMediExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MEDI, bool>>>();

        

        internal HisMestPeriodMediSO Query()
        {
            HisMestPeriodMediSO search = new HisMestPeriodMediSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMestPeriodMediExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMestPeriodMediExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMestPeriodMediExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMestPeriodMediExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listHisMestPeriodMediExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listHisMestPeriodMediExpression.Add(o => this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listHisMestPeriodMediExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }

                search.listHisMestPeriodMediExpression.AddRange(listHisMestPeriodMediExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMestPeriodMediExpression.Clear();
                search.listHisMestPeriodMediExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
