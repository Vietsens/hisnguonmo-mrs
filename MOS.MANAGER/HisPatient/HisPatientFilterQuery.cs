using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatient
{
    public class HisPatientFilterQuery : HisPatientFilter
    {
        public HisPatientFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PATIENT, bool>>> listHisPatientExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT, bool>>>();

        

        internal HisPatientSO Query()
        {
            HisPatientSO search = new HisPatientSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisPatientExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisPatientExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisPatientExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisPatientExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.PATIENT_CODE))
                {
                    search.listHisPatientExpression.Add(o => o.PATIENT_CODE == this.PATIENT_CODE);
                }
                if (!String.IsNullOrEmpty(this.PATIENT_NAME))
                {
                    search.listHisPatientExpression.Add(o => o.VIR_PATIENT_NAME.ToLower().Equals(this.PATIENT_NAME.ToLower()));
                }
                if (this.DOB.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.DOB == this.DOB.Value);
                }
                if (this.GENDER_ID.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.GENDER_ID == this.GENDER_ID.Value);
                }
                if (this.CAREER_ID.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.CAREER_ID.HasValue && o.CAREER_ID.Value == this.CAREER_ID.Value);
                }
                if (this.MILITARY_RANK_ID.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.MILITARY_RANK_ID.HasValue && o.MILITARY_RANK_ID.Value == this.MILITARY_RANK_ID.Value);
                }
                if (this.WORK_PLACE_ID.HasValue)
                {
                    search.listHisPatientExpression.Add(o => o.WORK_PLACE_ID.HasValue && o.WORK_PLACE_ID.Value == this.WORK_PLACE_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.PATIENT_CODE__EXACT))
                {
                    search.listHisPatientExpression.Add(o => o.PATIENT_CODE == this.PATIENT_CODE__EXACT);
                }

                search.listHisPatientExpression.AddRange(listHisPatientExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPatientExpression.Clear();
                search.listHisPatientExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
