using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCare
{
    public class HisCareFilterQuery : HisCareFilter
    {
        public HisCareFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_CARE, bool>>> listHisCareExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARE, bool>>>();

        

        internal HisCareSO Query()
        {
            HisCareSO search = new HisCareSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisCareExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisCareExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisCareExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisCareExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisCareExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisCareExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisCareExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisCareExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisCareExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.AWARENESS_ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.AWARENESS_ID.HasValue && o.AWARENESS_ID.Value == this.AWARENESS_ID.Value);
                }
                if (this.EXECUTE_TIME_FROM.HasValue)
                {
                    listHisCareExpression.Add(o => o.EXECUTE_TIME.Value >= this.EXECUTE_TIME_FROM.Value);
                }
                if (this.EXECUTE_TIME_TO.HasValue)
                {
                    listHisCareExpression.Add(o => o.EXECUTE_TIME.Value <= this.EXECUTE_TIME_TO.Value);
                }
                if (this.CARE_SUM_ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.CARE_SUM_ID.HasValue && o.CARE_SUM_ID.Value == this.CARE_SUM_ID.Value);
                }
                if (this.DHST_ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.DHST_ID.HasValue && o.DHST_ID.Value == this.DHST_ID.Value);
                }
                if (this.TRACKING_ID.HasValue)
                {
                    listHisCareExpression.Add(o => o.TRACKING_ID.HasValue && o.TRACKING_ID.Value == this.TRACKING_ID.Value);
                }
                
                search.listHisCareExpression.AddRange(listHisCareExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisCareExpression.Clear();
                search.listHisCareExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
