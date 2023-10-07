using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatment
{
    public class HisTreatmentFilterQuery : HisTreatmentFilter
    {
        public HisTreatmentFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT, bool>>> listHisTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT, bool>>>();



        internal HisTreatmentSO Query()
        {
            HisTreatmentSO search = new HisTreatmentSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTreatmentExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisTreatmentExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisTreatmentExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisTreatmentExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.ICD_CODE))
                {
                    listHisTreatmentExpression.Add(o => o.ICD_CODE == this.ICD_CODE);
                }
                if (this.ICD_CODEs != null)
                {
                    listHisTreatmentExpression.Add(o => !String.IsNullOrWhiteSpace(o.ICD_CODE) && this.ICD_CODEs.Contains(o.ICD_CODE));
                }
                if (this.PATIENT_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.PATIENT_ID == this.PATIENT_ID.Value);
                }
                if (this.TREATMENT_END_TYPE_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TREATMENT_END_TYPE_ID.HasValue && o.TREATMENT_END_TYPE_ID.Value == this.TREATMENT_END_TYPE_ID.Value);
                }
                if (this.TREATMENT_RESULT_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TREATMENT_RESULT_ID.HasValue && o.TREATMENT_RESULT_ID.Value == this.TREATMENT_RESULT_ID.Value);
                }
                if (this.IN_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.IN_TIME >= this.IN_TIME_FROM.Value);
                }
                if (this.IN_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.IN_TIME <= this.IN_TIME_TO.Value);
                }
                if (this.IS_OUT.HasValue && this.IS_OUT.Value)
                {
                    listHisTreatmentExpression.Add(o => o.OUT_TIME.HasValue);
                }
                if (this.IS_OUT.HasValue && !this.IS_OUT.Value)
                {
                    listHisTreatmentExpression.Add(o => !o.OUT_TIME.HasValue);
                }
                //if (this.PROGRAM_ID.HasValue)
                //{
                //    listHisTreatmentExpression.Add(o => o.PROGRAM_ID.HasValue && o.PROGRAM_ID.Value == this.PROGRAM_ID.Value);
                //}
                //if (this.PROGRAM_IDs != null)
                //{
                //    listHisTreatmentExpression.Add(o => o.PROGRAM_ID.HasValue && this.PROGRAM_IDs.Contains(o.PROGRAM_ID.Value));
                //}
                if (this.EMERGENCY_WTIME_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.EMERGENCY_WTIME_ID.HasValue && o.EMERGENCY_WTIME_ID.Value == this.EMERGENCY_WTIME_ID.Value);
                }
                if (this.EMERGENCY_WTIME_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.EMERGENCY_WTIME_ID.HasValue && this.EMERGENCY_WTIME_IDs.Contains(o.EMERGENCY_WTIME_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.STORE_CODE__END_WITH))
                {
                    listHisTreatmentExpression.Add(o => o.STORE_CODE != null && o.STORE_CODE.EndsWith(this.STORE_CODE__END_WITH));
                }
                if (!String.IsNullOrEmpty(this.STORE_CODE__START_WITH))
                {
                    listHisTreatmentExpression.Add(o => o.STORE_CODE != null && o.STORE_CODE.StartsWith(this.STORE_CODE__START_WITH));
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE__EXACT))
                {
                    listHisTreatmentExpression.Add(o => o.TREATMENT_CODE == this.TREATMENT_CODE__EXACT);
                }
                if (this.OWE_TYPE_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.OWE_TYPE_ID.HasValue && o.OWE_TYPE_ID.Value == this.OWE_TYPE_ID.Value);
                }
                if (this.OWE_TYPE_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.OWE_TYPE_ID.HasValue && this.OWE_TYPE_IDs.Contains(o.OWE_TYPE_ID.Value));
                }
                if (this.DEATH_WITHIN_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.DEATH_WITHIN_ID.HasValue && o.DEATH_WITHIN_ID.Value == this.DEATH_WITHIN_ID.Value);
                }
                if (this.DEATH_WITHIN_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.DEATH_WITHIN_ID.HasValue && this.DEATH_WITHIN_IDs.Contains(o.DEATH_WITHIN_ID.Value));
                }
                if (this.DEATH_CAUSE_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.DEATH_CAUSE_ID.HasValue && o.DEATH_CAUSE_ID.Value == this.DEATH_CAUSE_ID.Value);
                }
                if (this.DEATH_CAUSE_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.DEATH_CAUSE_ID.HasValue && this.DEATH_CAUSE_IDs.Contains(o.DEATH_CAUSE_ID.Value));
                }
                if (this.TRAN_PATI_FORM_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TRAN_PATI_FORM_ID.HasValue && o.TRAN_PATI_FORM_ID.Value == this.TRAN_PATI_FORM_ID.Value);
                }
                if (this.TRAN_PATI_FORM_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.TRAN_PATI_FORM_ID.HasValue && this.TRAN_PATI_FORM_IDs.Contains(o.TRAN_PATI_FORM_ID.Value));
                }
                if (this.TRAN_PATI_REASON_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TRAN_PATI_REASON_ID.HasValue && o.TRAN_PATI_REASON_ID.Value == this.TRAN_PATI_REASON_ID.Value);
                }
                if (this.TRAN_PATI_REASON_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.TRAN_PATI_REASON_ID.HasValue && this.TRAN_PATI_REASON_IDs.Contains(o.TRAN_PATI_REASON_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.APPOINTMENT_CODE__EXACT))
                {
                    listHisTreatmentExpression.Add(o => o.APPOINTMENT_CODE == this.APPOINTMENT_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TDL_PATIENT_CODE__EXACT))
                {
                    listHisTreatmentExpression.Add(o => o.TDL_PATIENT_CODE == this.TDL_PATIENT_CODE__EXACT);
                }
                if (this.IN_DATE_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.IN_DATE >= this.IN_DATE_FROM.Value);
                }
                if (this.IN_DATE_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.IN_DATE <= this.IN_DATE_TO.Value);
                }
                if (this.OUT_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.OUT_TIME >= this.OUT_TIME_FROM.Value);
                }
                if (this.OUT_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.OUT_TIME <= this.OUT_TIME_TO.Value);
                }

                if (this.END_ROOM_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.END_ROOM_ID.HasValue && this.END_ROOM_IDs.Contains(o.END_ROOM_ID.Value));
                }
                if (this.FEE_LOCK_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.FEE_LOCK_TIME.HasValue && o.FEE_LOCK_TIME.Value >= this.FEE_LOCK_TIME_FROM.Value);
                }
                if (this.FEE_LOCK_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.FEE_LOCK_TIME.HasValue && o.FEE_LOCK_TIME.Value <= this.FEE_LOCK_TIME_TO.Value);
                }
                if (this.DOB_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TDL_PATIENT_DOB <= this.DOB_TO.Value);
                }
                if (this.DOB_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TDL_PATIENT_DOB >= this.DOB_FROM.Value);
                }
                if (this.IS_LOCK_HEIN.HasValue && this.IS_LOCK_HEIN.Value)
                {
                    listHisTreatmentExpression.Add(o => o.IS_LOCK_HEIN.HasValue && o.IS_LOCK_HEIN.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_LOCK_HEIN.HasValue && !this.IS_LOCK_HEIN.Value)
                {
                    listHisTreatmentExpression.Add(o => !o.IS_LOCK_HEIN.HasValue || o.IS_LOCK_HEIN.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_PAUSE.HasValue && this.IS_PAUSE.Value)
                {
                    listHisTreatmentExpression.Add(o => o.IS_PAUSE.HasValue && o.IS_PAUSE.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_PAUSE.HasValue && !this.IS_PAUSE.Value)
                {
                    listHisTreatmentExpression.Add(o => !o.IS_PAUSE.HasValue || o.IS_PAUSE.Value != ManagerConstant.IS_TRUE);
                }
                if (this.HAS_DATA_STORE.HasValue && this.HAS_DATA_STORE.Value)
                {
                    listHisTreatmentExpression.Add(o => o.DATA_STORE_ID.HasValue);
                }
                if (this.HAS_DATA_STORE.HasValue && !this.HAS_DATA_STORE.Value)
                {
                    listHisTreatmentExpression.Add(o => !o.DATA_STORE_ID.HasValue);
                }
                if (this.DATA_STORE_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.DATA_STORE_ID.HasValue && this.DATA_STORE_IDs.Contains(o.DATA_STORE_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_CODE__EXACT))
                {
                    listHisTreatmentExpression.Add(o => o.TDL_PATIENT_CODE == this.PATIENT_CODE__EXACT);
                }
                if (this.END_DEPARTMENT_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.END_DEPARTMENT_ID.HasValue && this.END_DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID.Value));
                }
                if (this.PATIENT_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => this.PATIENT_IDs.Contains(o.PATIENT_ID));
                }
                if (this.IS_CHRONIC.HasValue && this.IS_CHRONIC.Value)
                {
                    listHisTreatmentExpression.Add(o => o.IS_CHRONIC.HasValue && o.IS_CHRONIC.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CHRONIC.HasValue && !this.IS_CHRONIC.Value)
                {
                    listHisTreatmentExpression.Add(o => !o.IS_CHRONIC.HasValue || o.IS_CHRONIC.Value != ManagerConstant.IS_TRUE);
                }
                if (!String.IsNullOrWhiteSpace(this.TDL_HEIN_CARD_NUMBER__EXACT))
                {
                    listHisTreatmentExpression.Add(o => !String.IsNullOrWhiteSpace(o.TDL_HEIN_CARD_NUMBER) && o.TDL_HEIN_CARD_NUMBER == this.TDL_HEIN_CARD_NUMBER__EXACT);
                }
                if (this.CLINICAL_IN_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.CLINICAL_IN_TIME >= this.CLINICAL_IN_TIME_FROM.Value);
                }
                if (this.CLINICAL_IN_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.CLINICAL_IN_TIME <= this.CLINICAL_IN_TIME_TO.Value);
                }
                if (this.APPOINTMENT_TIME_FROM.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.APPOINTMENT_TIME.HasValue && o.APPOINTMENT_TIME >= this.APPOINTMENT_TIME_FROM.Value);
                }
                if (this.APPOINTMENT_TIME_TO.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.APPOINTMENT_TIME.HasValue && o.APPOINTMENT_TIME <= this.APPOINTMENT_TIME_TO.Value);
                }
                if (this.KSK_CONTRACT_ID.HasValue)
                {
                    listHisTreatmentExpression.Add(o => o.TDL_KSK_CONTRACT_ID.HasValue && o.TDL_KSK_CONTRACT_ID.Value == this.KSK_CONTRACT_ID.Value);
                }
                if (this.KSK_CONTRACT_IDs != null)
                {
                    listHisTreatmentExpression.Add(o => o.TDL_KSK_CONTRACT_ID.HasValue && this.KSK_CONTRACT_IDs.Contains(o.TDL_KSK_CONTRACT_ID.Value));
                }

                search.listHisTreatmentExpression.AddRange(listHisTreatmentExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTreatmentExpression.Clear();
                search.listHisTreatmentExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
