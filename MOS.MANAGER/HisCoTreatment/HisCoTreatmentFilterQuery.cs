using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCoTreatment
{
    public class HisCoTreatmentFilterQuery : HisCoTreatmentFilter
    {
        public HisCoTreatmentFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_CO_TREATMENT, bool>>> listHisCoTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CO_TREATMENT, bool>>>();



        internal HisCoTreatmentSO Query()
        {
            HisCoTreatmentSO search = new HisCoTreatmentSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisCoTreatmentExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisCoTreatmentExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisCoTreatmentExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisCoTreatmentExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisCoTreatmentExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.DEPARTMENT_ID.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.DEPARTMENT_ID == this.DEPARTMENT_ID.Value);
                }
                if (this.DEPARTMENT_IDs != null)
                {
                    listHisCoTreatmentExpression.Add(o => this.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID));
                }
                if (this.DEPARTMENT_TRAN_ID.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.DEPARTMENT_TRAN_ID == this.DEPARTMENT_TRAN_ID.Value);
                }
                if (this.DEPARTMENT_TRAN_IDs != null)
                {
                    listHisCoTreatmentExpression.Add(o => this.DEPARTMENT_TRAN_IDs.Contains(o.DEPARTMENT_TRAN_ID));
                }
                if (this.START_TIME_FROM.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue && o.START_TIME.Value >= this.START_TIME_FROM.Value);
                }
                if (this.START_TIME_TO.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue && o.START_TIME.Value <= this.START_TIME_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME.Value >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    listHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME.Value <= this.FINISH_TIME_TO.Value);
                }

                if (this.HAS_FINISH_TIME.HasValue)
                {
                    if (this.HAS_FINISH_TIME.Value)
                    {
                        listHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue);
                    }
                    else
                    {
                        listHisCoTreatmentExpression.Add(o => !o.FINISH_TIME.HasValue);
                    }
                }
                if (this.HAS_START_TIME.HasValue)
                {
                    if (this.HAS_START_TIME.Value)
                    {
                        listHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue);
                    }
                    else
                    {
                        listHisCoTreatmentExpression.Add(o => !o.START_TIME.HasValue);
                    }
                }

                search.listHisCoTreatmentExpression.AddRange(listHisCoTreatmentExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisCoTreatmentExpression.Clear();
                search.listHisCoTreatmentExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
