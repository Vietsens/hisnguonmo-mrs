using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePaty
{
    public class HisServicePatyFilterQuery : HisServicePatyFilter
    {
        public HisServicePatyFilterQuery()
            : base()
        {
        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PATY, bool>>> listHisServicePatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PATY, bool>>>();

        

        internal HisServicePatySO Query()
        {
            HisServicePatySO search = new HisServicePatySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisServicePatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisServicePatyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisServicePatyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisServicePatyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_ID.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    search.listHisServicePatyExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    search.listHisServicePatyExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.IN_ACTIVE_TIME.HasValue && this.IN_ACTIVE_TIME.Value)
                {
                    search.listHisServicePatyExpression.Add(o => 
                        ((!o.FROM_TIME.HasValue || o.FROM_TIME.Value <= this.INSTRUCTION_TIME.Value) && (!o.TO_TIME.HasValue || o.TO_TIME.Value >= this.INSTRUCTION_TIME.Value))
                        || ((!o.TREATMENT_FROM_TIME.HasValue || o.TREATMENT_FROM_TIME.Value <= this.TREATMENT_TIME) && (!o.TREATMENT_TO_TIME.HasValue || o.TREATMENT_TO_TIME.Value >= this.TREATMENT_TIME))
                        );
                    
                }
                if (this.BRANCH_ID.HasValue)
                {
                    listHisServicePatyExpression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }
                if (this.BRANCH_IDs != null)
                {
                    listHisServicePatyExpression.Add(o => this.BRANCH_IDs.Contains(o.BRANCH_ID));
                }

                search.listHisServicePatyExpression.AddRange(listHisServicePatyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServicePatyExpression.Clear();
                search.listHisServicePatyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
