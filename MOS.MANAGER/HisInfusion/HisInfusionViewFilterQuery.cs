using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusion
{
    public class HisInfusionViewFilterQuery : HisInfusionViewFilter
    {
        public HisInfusionViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION, bool>>> listVHisInfusionExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION, bool>>>();

        

        internal HisInfusionSO Query()
        {
            HisInfusionSO search = new HisInfusionSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisInfusionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisInfusionExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisInfusionExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisInfusionExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisInfusionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.INFUSION_SUM_ID.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.INFUSION_SUM_ID == this.INFUSION_SUM_ID.Value);
                }
                if (this.INFUSION_SUM_IDs != null)
                {
                    listVHisInfusionExpression.Add(o => this.INFUSION_SUM_IDs.Contains(o.INFUSION_SUM_ID));
                }
                if (this.MEDICINE_IDs!=null)
                {
                    listVHisInfusionExpression.Add(o => o.MEDICINE_ID.HasValue&&this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisInfusionExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }

                search.listVHisInfusionExpression.AddRange(listVHisInfusionExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisInfusionExpression.Clear();
                search.listVHisInfusionExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
