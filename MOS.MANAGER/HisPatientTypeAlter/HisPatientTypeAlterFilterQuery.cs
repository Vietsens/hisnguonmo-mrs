using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    public class HisPatientTypeAlterFilterQuery : HisPatientTypeAlterFilter
    {
        public HisPatientTypeAlterFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALTER, bool>>> listHisPatientTypeAlterExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALTER, bool>>>();

        

        internal HisPatientTypeAlterSO Query()
        {
            HisPatientTypeAlterSO search = new HisPatientTypeAlterSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisPatientTypeAlterExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPatientTypeAlterExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPatientTypeAlterExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPatientTypeAlterExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    search.listHisPatientTypeAlterExpression.Add(o => this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID));
                }
                if (this.TREATMENT_IDs != null)
                {
                    search.listHisPatientTypeAlterExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.TDL_PATIENT_ID.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.TDL_PATIENT_ID == this.TDL_PATIENT_ID.Value);
                }
                if (this.TREATMENT_TYPE_ID.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.TREATMENT_TYPE_ID == this.TREATMENT_TYPE_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER__EXACT))
                {
                    listHisPatientTypeAlterExpression.Add(o => o.HEIN_CARD_NUMBER == this.HEIN_CARD_NUMBER__EXACT);
                }
                if (!String.IsNullOrEmpty(this.HAS_BIRTH_CERTIFICATE__EXACT))
                {
                    listHisPatientTypeAlterExpression.Add(o => o.HAS_BIRTH_CERTIFICATE == this.HAS_BIRTH_CERTIFICATE__EXACT);
                }
                if (this.LOG_TIME_TO.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.LOG_TIME <= this.LOG_TIME_TO.Value);
                }
                if (this.LOG_TIME_FROM.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.LOG_TIME >= this.LOG_TIME_FROM.Value);
                }
                if (this.TDL_PATIENT_ID__NOT_EQUAL.HasValue)
                {
                    listHisPatientTypeAlterExpression.Add(o => o.TDL_PATIENT_ID != this.TDL_PATIENT_ID__NOT_EQUAL.Value);
                }

                search.listHisPatientTypeAlterExpression.AddRange(listHisPatientTypeAlterExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPatientTypeAlterExpression.Clear();
                search.listHisPatientTypeAlterExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
