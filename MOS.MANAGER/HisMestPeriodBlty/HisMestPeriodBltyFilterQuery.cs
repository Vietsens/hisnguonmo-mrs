using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlty
{
    public class HisMestPeriodBltyFilterQuery : HisMestPeriodBltyFilter
    {
        public HisMestPeriodBltyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLTY, bool>>> listHisMestPeriodBltyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLTY, bool>>>();

        

        internal HisMestPeriodBltySO Query()
        {
            HisMestPeriodBltySO search = new HisMestPeriodBltySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisMestPeriodBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMestPeriodBltyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMestPeriodBltyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMestPeriodBltyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisMestPeriodBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listHisMestPeriodBltyExpression.Add(o => this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID));
                }
                if (this.BLOOD_TYPE_ID.HasValue)
                {
                    listHisMestPeriodBltyExpression.Add(o => o.BLOOD_TYPE_ID == this.BLOOD_TYPE_ID.Value);
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listHisMestPeriodBltyExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }

                search.listHisMestPeriodBltyExpression.AddRange(listHisMestPeriodBltyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMestPeriodBltyExpression.Clear();
                search.listHisMestPeriodBltyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
