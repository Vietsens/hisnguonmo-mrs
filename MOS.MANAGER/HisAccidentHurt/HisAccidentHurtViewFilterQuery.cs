using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    public class HisAccidentHurtViewFilterQuery : HisAccidentHurtViewFilter
    {
        public HisAccidentHurtViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ACCIDENT_HURT, bool>>> listVHisAccidentHurtExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACCIDENT_HURT, bool>>>();

        

        internal HisAccidentHurtSO Query()
        {
            HisAccidentHurtSO search = new HisAccidentHurtSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisAccidentHurtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisAccidentHurtExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisAccidentHurtExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisAccidentHurtExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.ACCIDENT_HURT_TYPE_ID.HasValue)
                {
                    listVHisAccidentHurtExpression.Add(o => o.ACCIDENT_HURT_TYPE_ID == this.ACCIDENT_HURT_TYPE_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisAccidentHurtExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAccidentHurtExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_BODY_PART_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_BODY_PART_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_CARE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_CARE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_HELMET_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_HELMET_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_HURT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_HURT_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_LOCATION_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_LOCATION_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_POISON_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_POISON_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_RESULT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_RESULT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_VEHICLE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_VEHICLE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_FIRST_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_GENDER_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_LAST_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TREATMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_ADDRESS.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisAccidentHurtExpression.AddRange(listVHisAccidentHurtExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisAccidentHurtExpression.Clear();
                search.listVHisAccidentHurtExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
