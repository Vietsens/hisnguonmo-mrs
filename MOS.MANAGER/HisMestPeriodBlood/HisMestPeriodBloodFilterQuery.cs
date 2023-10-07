using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlood
{
    public class HisMestPeriodBloodFilterQuery : HisMestPeriodBloodFilter
    {
        public HisMestPeriodBloodFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLOOD, bool>>> listHisMestPeriodBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLOOD, bool>>>();

        

        internal HisMestPeriodBloodSO Query()
        {
            HisMestPeriodBloodSO search = new HisMestPeriodBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisMestPeriodBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMestPeriodBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMestPeriodBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMestPeriodBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisMestPeriodBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BLOOD_ID.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.BLOOD_ID == this.BLOOD_ID.Value);
                }
                if (this.BLOOD_IDs != null)
                {
                    listHisMestPeriodBloodExpression.Add(o => this.BLOOD_IDs.Contains(o.BLOOD_ID));
                }
                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listHisMestPeriodBloodExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listHisMestPeriodBloodExpression.Add(o => this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID));
                }

                search.listHisMestPeriodBloodExpression.AddRange(listHisMestPeriodBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMestPeriodBloodExpression.Clear();
                search.listHisMestPeriodBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
