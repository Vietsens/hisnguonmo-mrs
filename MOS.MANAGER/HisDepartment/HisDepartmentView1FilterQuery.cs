using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartment
{
    public class HisDepartmentView1FilterQuery : HisDepartmentView1Filter
    {
        public HisDepartmentView1FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_1, bool>>> listVHisDepartment1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPARTMENT_1, bool>>>();

        

        internal HisDepartmentSO Query()
        {
            HisDepartmentSO search = new HisDepartmentSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisDepartment1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisDepartment1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisDepartment1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisDepartment1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisDepartment1Expression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.G_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.BRANCH_ID.HasValue)
                {
                    listVHisDepartment1Expression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }
                if (this.BRANCH_IDs != null)
                {
                    listVHisDepartment1Expression.Add(o => this.BRANCH_IDs.Contains(o.BRANCH_ID));
                }
                if (this.IS_CLINICAL.HasValue && this.IS_CLINICAL.Value)
                {
                    listVHisDepartment1Expression.Add(o => o.IS_CLINICAL.HasValue && o.IS_CLINICAL.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CLINICAL.HasValue && !this.IS_CLINICAL.Value)
                {
                    listVHisDepartment1Expression.Add(o => !o.IS_CLINICAL.HasValue || o.IS_CLINICAL.Value != ManagerConstant.IS_TRUE);
                }

                search.listVHisDepartment1Expression.AddRange(listVHisDepartment1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisDepartment1Expression.Clear();
                search.listVHisDepartment1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
