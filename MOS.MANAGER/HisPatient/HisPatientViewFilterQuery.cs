using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatient
{
    public class HisPatientViewFilterQuery : HisPatientViewFilter
    {
       public HisPatientViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT, bool>>> listVHisPatientExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT, bool>>>();

        

        internal HisPatientSO Query()
        {
            HisPatientSO search = new HisPatientSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisPatientExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisPatientExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisPatientExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisPatientExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.PATIENT_CODE))
                {
                    search.listVHisPatientExpression.Add(o => o.PATIENT_CODE != null && o.PATIENT_CODE.ToLower().Contains(this.PATIENT_CODE.ToLower()));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_NAME))
                {
                    search.listVHisPatientExpression.Add(o => o.VIR_PATIENT_NAME != null && o.VIR_PATIENT_NAME.ToLower().Contains(this.PATIENT_NAME.ToLower()));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_NAME__EXACT))
                {
                    search.listVHisPatientExpression.Add(o => o.VIR_PATIENT_NAME != null && o.VIR_PATIENT_NAME.Equals(this.PATIENT_NAME__EXACT));
                }
                if (this.DOB.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.DOB == this.DOB.Value);
                }
                if (this.GENDER_ID.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.GENDER_ID == this.GENDER_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    search.listVHisPatientExpression.Add(o => o.PATIENT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.VIR_PATIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PHONE.ToLower().Contains(this.KEY_WORD)
                        || o.PROVINCE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.COMMUNE_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (this.GENDER_IDs != null)
                {
                    search.listVHisPatientExpression.Add(o => this.GENDER_IDs.Contains(o.GENDER_ID));
                }
                if (this.DOB_FROM.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.DOB >= this.DOB_FROM.Value);
                }
                if (this.DOB_TO.HasValue)
                {
                    search.listVHisPatientExpression.Add(o => o.DOB <= this.DOB_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.PATIENT_CODE__EXACT))
                {
                    search.listVHisPatientExpression.Add(o => o.PATIENT_CODE == this.PATIENT_CODE__EXACT);
                }

                search.listVHisPatientExpression.AddRange(listVHisPatientExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisPatientExpression.Clear();
                search.listVHisPatientExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
