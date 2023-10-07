using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCoTreatment
{
    public class HisCoTreatmentViewFilterQuery : HisCoTreatmentViewFilter
    {
        public HisCoTreatmentViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_CO_TREATMENT, bool>>> listVHisCoTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CO_TREATMENT, bool>>>();



        internal HisCoTreatmentSO Query()
        {
            HisCoTreatmentSO search = new HisCoTreatmentSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisCoTreatmentExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisCoTreatmentExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisCoTreatmentExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisCoTreatmentExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisCoTreatmentExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.DEPARTMENT_ID.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.DEPARTMENT_ID == this.DEPARTMENT_ID.Value);
                }
                if (this.DEPARTMENT_IDs != null)
                {
                    listVHisCoTreatmentExpression.Add(o => this.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID));
                }
                if (this.DEPARTMENT_TRAN_ID.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.DEPARTMENT_TRAN_ID == this.DEPARTMENT_TRAN_ID.Value);
                }
                if (this.DEPARTMENT_TRAN_IDs != null)
                {
                    listVHisCoTreatmentExpression.Add(o => this.DEPARTMENT_TRAN_IDs.Contains(o.DEPARTMENT_TRAN_ID));
                }
                if (this.START_TIME_FROM.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue && o.START_TIME.Value >= this.START_TIME_FROM.Value);
                }
                if (this.START_TIME_TO.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue && o.START_TIME.Value <= this.START_TIME_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME.Value >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME.Value <= this.FINISH_TIME_TO.Value);
                }

                if (this.HAS_FINISH_TIME.HasValue)
                {
                    if (this.HAS_FINISH_TIME.Value)
                    {
                        listVHisCoTreatmentExpression.Add(o => o.FINISH_TIME.HasValue);
                    }
                    else
                    {
                        listVHisCoTreatmentExpression.Add(o => !o.FINISH_TIME.HasValue);
                    }
                }
                if (this.HAS_START_TIME.HasValue)
                {
                    if (this.HAS_START_TIME.Value)
                    {
                        listVHisCoTreatmentExpression.Add(o => o.START_TIME.HasValue);
                    }
                    else
                    {
                        listVHisCoTreatmentExpression.Add(o => !o.START_TIME.HasValue);
                    }
                }
                if (this.CURRENT_DEPARTMENT_ID.HasValue)
                {
                    listVHisCoTreatmentExpression.Add(o => o.CURRENT_DEPARTMENT_ID == this.CURRENT_DEPARTMENT_ID.Value);
                }
                if (this.CURRENT_DEPARTMENT_IDs != null)
                {
                    listVHisCoTreatmentExpression.Add(o => this.CURRENT_DEPARTMENT_IDs.Contains(o.CURRENT_DEPARTMENT_ID));
                }
                if (!String.IsNullOrWhiteSpace(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisCoTreatmentExpression.Add(o => o.APP_CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.CURRENT_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.CURRENT_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.GROUP_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisCoTreatmentExpression.AddRange(listVHisCoTreatmentExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisCoTreatmentExpression.Clear();
                search.listVHisCoTreatmentExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
