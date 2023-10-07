using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    public class HisAccidentHurtFilterQuery : HisAccidentHurtFilter
    {
        public HisAccidentHurtFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT, bool>>> listHisAccidentHurtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_HURT, bool>>>();

        internal HisAccidentHurtSO Query()
        {
            HisAccidentHurtSO search = new HisAccidentHurtSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAccidentHurtExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAccidentHurtExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAccidentHurtExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.ACCIDENT_HURT_TYPE_ID.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.ACCIDENT_HURT_TYPE_ID == this.ACCIDENT_HURT_TYPE_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.ACCIDENT_CARE_ID.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.ACCIDENT_CARE_ID.HasValue && o.ACCIDENT_CARE_ID.Value == this.ACCIDENT_CARE_ID.Value);
                }
                if (this.ACCIDENT_CARE_IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => this.ACCIDENT_CARE_IDs.Contains(o.ACCIDENT_CARE_ID.Value));
                }
                if (this.ACCIDENT_HELMET_ID.HasValue)
                {
                    listHisAccidentHurtExpression.Add(o => o.ACCIDENT_HELMET_ID.HasValue && o.ACCIDENT_HELMET_ID.Value == this.ACCIDENT_HELMET_ID.Value);
                }
                if (this.ACCIDENT_HELMET_IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => o.ACCIDENT_HELMET_ID.HasValue && this.ACCIDENT_HELMET_IDs.Contains(o.ACCIDENT_HELMET_ID.Value));
                }
                
                if (this.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    search.listHisAccidentHurtExpression.Add(o => o.EXECUTE_DEPARTMENT_ID == this.EXECUTE_DEPARTMENT_ID.Value);
                }
                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    search.listHisAccidentHurtExpression.Add(o => o.EXECUTE_DEPARTMENT_ID == this.EXECUTE_DEPARTMENT_ID.Value);
                }
                if (this.EXECUTE_DEPARTMENT_IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => o.EXECUTE_DEPARTMENT_ID.HasValue && this.EXECUTE_DEPARTMENT_IDs.Contains(o.EXECUTE_DEPARTMENT_ID.Value));
                }
                if (this.EXECUTE_ROOM_IDs != null)
                {
                    listHisAccidentHurtExpression.Add(o => EXECUTE_ROOM_ID.HasValue && this.EXECUTE_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID.Value));
                }

                search.listHisAccidentHurtExpression.AddRange(listHisAccidentHurtExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisAccidentHurtExpression.Clear();
                search.listHisAccidentHurtExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
