using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCard
{
    public class HisCardViewFilterQuery : HisCardViewFilter
    {
        public HisCardViewFilterQuery()
            : base()
        {

        }


        internal List<System.Linq.Expressions.Expression<Func<V_HIS_CARD, bool>>> listVHisCardExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CARD, bool>>>();


        internal HisCardSO Query()
        {
            HisCardSO search = new HisCardSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisCardExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisCardExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisCardExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisCardExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.PATIENT_ID.HasValue)
                {
                    search.listVHisCardExpression.Add(o => o.PATIENT_ID == this.PATIENT_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.SERVICE_CODE__EXACT))
                {
                    search.listVHisCardExpression.Add(o => o.SERVICE_CODE == this.SERVICE_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.CODE__EXACT))
                {
                    search.listVHisCardExpression.Add(o => o.SERVICE_CODE == this.CODE__EXACT || o.CARD_CODE == this.CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.CARD_CODE__EXACT))
                {
                    search.listVHisCardExpression.Add(o => o.CARD_CODE == this.CARD_CODE__EXACT);
                }
                
                search.listVHisCardExpression.AddRange(listVHisCardExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisCardExpression.Clear();
                search.listVHisCardExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
