using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;


namespace MOS.MANAGER.HisServicePaty
{
    public class HisServicePatyViewFilterQuery : HisServicePatyViewFilter
    {
        public HisServicePatyViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PATY, bool>>> listVHisServicePatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PATY, bool>>>();

        internal HisServicePatySO Query()
        {
            HisServicePatySO search = new HisServicePatySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisServicePatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisServicePatyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisServicePatyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisServicePatyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_TYPE_ID.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.SERVICE_TYPE_ID == this.SERVICE_TYPE_ID.Value);
                }
                if (this.TREATMENT_TIME.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.TREATMENT_FROM_TIME <= this.TREATMENT_TIME && o.TREATMENT_TO_TIME >= this.TREATMENT_TIME);
                }
                if (this.SERVICE_ID.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    search.listVHisServicePatyExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.IN_ACTIVE_TIME.HasValue)
                {
                    long? now = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now);
                    if (this.IN_ACTIVE_TIME.Value)
                    {
                        search.listVHisServicePatyExpression.Add(o => (!o.FROM_TIME.HasValue || o.FROM_TIME.Value <= now) && (!o.TO_TIME.HasValue || o.TO_TIME.Value >= now));
                    }
                    else
                    {
                        search.listVHisServicePatyExpression.Add(o => (o.FROM_TIME.HasValue && o.FROM_TIME.Value > now) || (o.TO_TIME.HasValue && o.TO_TIME.Value < now));
                    }
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisServicePatyExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.BRANCH_ID.HasValue)
                {
                    search.listVHisServicePatyExpression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }

                search.listVHisServicePatyExpression.AddRange(listVHisServicePatyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisServicePatyExpression.Clear();
                search.listVHisServicePatyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
