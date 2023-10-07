using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartmentTran
{
    public class HisDepartmentTranFilterQuery : HisDepartmentTranFilter
    {
        public HisDepartmentTranFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT_TRAN, bool>>> listHisDepartmentTranExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEPARTMENT_TRAN, bool>>>();



        internal HisDepartmentTranSO Query()
        {
            HisDepartmentTranSO search = new HisDepartmentTranSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisDepartmentTranExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisDepartmentTranExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisDepartmentTranExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisDepartmentTranExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisDepartmentTranExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.DEPARTMENT_ID.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.DEPARTMENT_ID == this.DEPARTMENT_ID.Value);
                }
                if (this.DEPARTMENT_IDs != null)
                {
                    listHisDepartmentTranExpression.Add(o => this.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.DEPARTMENT_IN_TIME_FROM.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_IN_TIME.Value >= this.DEPARTMENT_IN_TIME_FROM.Value);
                }
                if (this.DEPARTMENT_IN_TIME_TO.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_IN_TIME.Value <= this.DEPARTMENT_IN_TIME_TO.Value);
                }
                if (this.PREVIOUS_ID.HasValue)
                {
                    listHisDepartmentTranExpression.Add(o => o.PREVIOUS_ID.HasValue && o.PREVIOUS_ID.Value == this.PREVIOUS_ID.Value);
                }
                if (this.PREVIOUS_IDs != null)
                {
                    listHisDepartmentTranExpression.Add(o => o.PREVIOUS_ID.HasValue && this.PREVIOUS_IDs.Contains(o.PREVIOUS_ID.Value));
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisDepartmentTranExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                search.listHisDepartmentTranExpression.AddRange(listHisDepartmentTranExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisDepartmentTranExpression.Clear();
                search.listHisDepartmentTranExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
