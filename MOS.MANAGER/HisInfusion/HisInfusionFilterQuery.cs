using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusion
{
    public class HisInfusionFilterQuery : HisInfusionFilter
    {
        public HisInfusionFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_INFUSION, bool>>> listHisInfusionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INFUSION, bool>>>();

        

        internal HisInfusionSO Query()
        {
            HisInfusionSO search = new HisInfusionSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisInfusionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisInfusionExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisInfusionExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisInfusionExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisInfusionExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.INFUSION_SUM_ID.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.INFUSION_SUM_ID == this.INFUSION_SUM_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listHisInfusionExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.INFUSION_SUM_IDs != null)
                {
                    listHisInfusionExpression.Add(o => this.INFUSION_SUM_IDs.Contains(o.INFUSION_SUM_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listHisInfusionExpression.Add(o =>o.MEDICINE_ID.HasValue&& this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }

                search.listHisInfusionExpression.AddRange(listHisInfusionExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisInfusionExpression.Clear();
                search.listHisInfusionExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
