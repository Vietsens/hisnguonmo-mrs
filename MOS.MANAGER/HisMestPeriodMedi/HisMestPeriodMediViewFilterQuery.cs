using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMedi
{
    public class HisMestPeriodMediViewFilterQuery : HisMestPeriodMediViewFilter
    {
        public HisMestPeriodMediViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MEDI, bool>>> listVHisMestPeriodMediExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MEDI, bool>>>();

        

        internal HisMestPeriodMediSO Query()
        {
            HisMestPeriodMediSO search = new HisMestPeriodMediSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisMestPeriodMediExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMestPeriodMediExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMestPeriodMediExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.BID_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.BID_ID.HasValue && BID_ID.Value == this.ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.SUPPLIER_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.SUPPLIER_ID.HasValue && o.SUPPLIER_ID.Value == this.SUPPLIER_ID.Value);
                }
                if (this.SUPPLIER_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_IDs.Contains(o.SUPPLIER_ID.Value));
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listVHisMestPeriodMediExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisMestPeriodMediExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                
                search.listVHisMestPeriodMediExpression.AddRange(listVHisMestPeriodMediExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMestPeriodMediExpression.Clear();
                search.listVHisMestPeriodMediExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
