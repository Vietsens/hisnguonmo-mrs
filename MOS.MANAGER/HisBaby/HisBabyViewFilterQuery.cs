using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBaby
{
    public class HisBabyViewFilterQuery : HisBabyViewFilter
    {
        public HisBabyViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BABY, bool>>> listVHisBabyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BABY, bool>>>();



        internal HisBabySO Query()
        {
            HisBabySO search = new HisBabySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisBabyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBabyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBabyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBabyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBabyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion
                if (this.TREATMENT_IDs != null)
                {
                    listVHisBabyExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.BORN_POSITION_IDs != null)
                {
                    listVHisBabyExpression.Add(o => o.BORN_POSITION_ID.HasValue && this.BORN_POSITION_IDs.Contains(o.BORN_POSITION_ID ?? 0));
                }
                if (this.BORN_TYPE_IDs != null)
                {
                    listVHisBabyExpression.Add(o => o.BORN_TYPE_ID.HasValue && this.BORN_TYPE_IDs.Contains(o.BORN_TYPE_ID ?? 0));
                }
                if (this.BORN_RESULT_IDs != null)
                {
                    listVHisBabyExpression.Add(o => o.BORN_RESULT_ID.HasValue && this.BORN_RESULT_IDs.Contains(o.BORN_RESULT_ID ?? 0));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisBabyExpression.Add(o => this.TREATMENT_ID == o.TREATMENT_ID);
                }
                if (this.BORN_POSITION_ID.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.BORN_POSITION_ID.HasValue && this.BORN_POSITION_ID.Value == o.BORN_POSITION_ID.Value);
                }
                if (this.BORN_TYPE_ID.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.BORN_TYPE_ID.HasValue && this.BORN_TYPE_ID.Value == o.BORN_TYPE_ID.Value);
                }
                if (this.BORN_RESULT_ID.HasValue)
                {
                    listVHisBabyExpression.Add(o => o.BORN_RESULT_ID.HasValue && this.BORN_RESULT_ID.Value == o.BORN_RESULT_ID.Value);
                }

                search.listVHisBabyExpression.AddRange(listVHisBabyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBabyExpression.Clear();
                search.listVHisBabyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
