using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartmentTran
{
    public class HisDepartmentTranViewFilterQuery : HisDepartmentTranViewFilter
    {
        public HisDepartmentTranViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_TRAN, bool>>> listVHisDepartmentTranExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_TRAN, bool>>>();



        internal HisDepartmentTranSO Query()
        {
            HisDepartmentTranSO search = new HisDepartmentTranSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisDepartmentTranExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisDepartmentTranExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisDepartmentTranExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisDepartmentTranExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisDepartmentTranExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TREATMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_SUB_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_TEXT.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.IS_RECEIVE.HasValue && this.IS_RECEIVE.Value)
                {
                    listVHisDepartmentTranExpression.Add(o => o.DEPARTMENT_IN_TIME.HasValue);
                }
                if (this.IS_RECEIVE.HasValue && !this.IS_RECEIVE.Value)
                {
                    listVHisDepartmentTranExpression.Add(o => !o.DEPARTMENT_IN_TIME.HasValue);
                }
                if (this.DEPARTMENT_ID.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.DEPARTMENT_ID == this.DEPARTMENT_ID.Value);
                }
                if (this.DEPARTMENT_IN_TIME_FROM.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_IN_TIME.Value >= this.DEPARTMENT_IN_TIME_FROM.Value);
                }
                if (this.DEPARTMENT_IN_TIME_TO.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_IN_TIME <= this.DEPARTMENT_IN_TIME_TO.Value);
                }
                if (this.DEPARTMENT_IDs != null)
                {
                    listVHisDepartmentTranExpression.Add(o => this.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID));
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisDepartmentTranExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.PREVIOUS_ID.HasValue)
                {
                    listVHisDepartmentTranExpression.Add(o => o.PREVIOUS_ID.HasValue && o.PREVIOUS_ID.Value == this.PREVIOUS_ID.Value);
                }
                if (this.PREVIOUS_IDs != null)
                {
                    listVHisDepartmentTranExpression.Add(o => o.PREVIOUS_ID.HasValue && this.PREVIOUS_IDs.Contains(o.PREVIOUS_ID.Value));
                }
                search.listVHisDepartmentTranExpression.AddRange(listVHisDepartmentTranExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisDepartmentTranExpression.Clear();
                search.listVHisDepartmentTranExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
