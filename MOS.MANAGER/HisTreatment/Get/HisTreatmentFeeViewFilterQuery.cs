using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatment
{
    public class HisTreatmentFeeViewFilterQuery : HisTreatmentFeeViewFilter
    {
        public HisTreatmentFeeViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE, bool>>> listVHisTreatmentFeeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE, bool>>>();

        

        internal HisTreatmentSO Query()
        {
            HisTreatmentSO search = new HisTreatmentSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisTreatmentFeeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.PATIENT_ID.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.PATIENT_ID == this.PATIENT_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.VIR_PATIENT_NAME))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_PATIENT_NAME.ToLower().Contains(this.VIR_PATIENT_NAME.ToLower().Trim()));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_CODE))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_PATIENT_CODE.ToLower().Contains(this.PATIENT_CODE.ToLower().Trim()));
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TREATMENT_CODE.ToLower().Contains(this.TREATMENT_CODE.ToLower().Trim()));
                }
                if (this.IS_LOCK_HEIN.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IS_LOCK_HEIN.HasValue && o.IS_LOCK_HEIN.Value == this.IS_LOCK_HEIN.Value);
                }
                if (this.TOTAL_HEIN_PRICE__GREATER_THAN.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TOTAL_HEIN_PRICE.HasValue && o.TOTAL_HEIN_PRICE.Value > this.TOTAL_HEIN_PRICE__GREATER_THAN.Value);
                }
                if (this.PATIENT_IDs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => this.PATIENT_IDs.Contains(o.PATIENT_ID));
                }

                if (this.IS_PAUSE.HasValue && this.IS_PAUSE.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IS_PAUSE.HasValue && o.IS_PAUSE.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_PAUSE.HasValue && !this.IS_PAUSE.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => !o.IS_PAUSE.HasValue || o.IS_PAUSE.Value != ManagerConstant.IS_TRUE);
                }
                if (this.FEE_LOCK_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.FEE_LOCK_TIME.HasValue && o.FEE_LOCK_TIME.Value >= this.FEE_LOCK_TIME_FROM.Value);
                }
                if (this.FEE_LOCK_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.FEE_LOCK_TIME.HasValue && o.FEE_LOCK_TIME.Value <= this.FEE_LOCK_TIME_TO.Value);
                }
                if (this.IN_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IN_TIME >= this.IN_TIME_FROM.Value);
                }
                if (this.IN_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IN_TIME <= this.IN_TIME_TO.Value);
                }
                if (this.OUT_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.OUT_TIME.HasValue && o.OUT_TIME >= this.OUT_TIME_FROM.Value);
                }
                if (this.OUT_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.OUT_TIME.HasValue && o.OUT_TIME <= this.OUT_TIME_TO.Value);
                }
                if (this.DOB_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_PATIENT_DOB <= this.DOB_TO.Value);
                }
                if (this.DOB_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_PATIENT_DOB >= this.DOB_FROM.Value);
                }
                if (this.END_ROOM_IDs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.END_ROOM_ID.HasValue && this.END_ROOM_IDs.Contains(o.END_ROOM_ID.Value));
                }

                if (this.IS_OUT.HasValue && this.IS_OUT.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.OUT_TIME.HasValue);
                }
                if (this.IS_OUT.HasValue && !this.IS_OUT.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => !o.OUT_TIME.HasValue);
                }
                if (this.TREATMENT_END_TYPE_ID != null)
                {
                    listVHisTreatmentFeeExpression.Add(s => s.TREATMENT_END_TYPE_ID == this.TREATMENT_END_TYPE_ID);
                }
                if (this.HAS_DATA_STORE.HasValue && this.HAS_DATA_STORE.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.DATA_STORE_ID.HasValue);
                }
                if (this.HAS_DATA_STORE.HasValue && !this.HAS_DATA_STORE.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => !o.DATA_STORE_ID.HasValue);
                }
                if (this.DATA_STORE_IDs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.DATA_STORE_ID.HasValue && this.DATA_STORE_IDs.Contains(o.DATA_STORE_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_CODE__EXACT))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_PATIENT_CODE == this.PATIENT_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE__EXACT))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TREATMENT_CODE == this.TREATMENT_CODE__EXACT);
                }
                if (this.STORE_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.STORE_TIME.HasValue && o.STORE_TIME >= this.STORE_TIME_FROM.Value);
                }
                if (this.STORE_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.STORE_TIME.HasValue && o.STORE_TIME <= this.STORE_TIME_TO.Value);
                }
                if (this.END_DEPARTMENT_IDs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.END_DEPARTMENT_ID.HasValue && this.END_DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID.Value));
                }
                if (this.IS_CHRONIC.HasValue && this.IS_CHRONIC.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IS_CHRONIC.HasValue && o.IS_CHRONIC.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CHRONIC.HasValue && !this.IS_CHRONIC.Value)
                {
                    listVHisTreatmentFeeExpression.Add(o => !o.IS_CHRONIC.HasValue || o.IS_CHRONIC.Value != ManagerConstant.IS_TRUE);
                }
                if (!String.IsNullOrWhiteSpace(this.TDL_HEIN_CARD_NUMBER__EXACT))
                {
                    listVHisTreatmentFeeExpression.Add(o => !String.IsNullOrWhiteSpace(o.TDL_HEIN_CARD_NUMBER) && o.TDL_HEIN_CARD_NUMBER == this.TDL_HEIN_CARD_NUMBER__EXACT);
                }
                if (this.CLINICAL_IN_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.CLINICAL_IN_TIME >= this.CLINICAL_IN_TIME_FROM.Value);
                }
                if (this.CLINICAL_IN_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.CLINICAL_IN_TIME <= this.CLINICAL_IN_TIME_TO.Value);
                }
                if (this.IN_DATE_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IN_DATE >= this.IN_DATE_FROM.Value);
                }
                if (this.IN_DATE_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.IN_DATE <= this.IN_DATE_TO.Value);
                }
                if (this.APPOINTMENT_TIME_FROM.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.APPOINTMENT_TIME.HasValue && o.APPOINTMENT_TIME >= this.APPOINTMENT_TIME_FROM.Value);
                }
                if (this.APPOINTMENT_TIME_TO.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.APPOINTMENT_TIME.HasValue && o.APPOINTMENT_TIME <= this.APPOINTMENT_TIME_TO.Value);
                }

                if (!String.IsNullOrEmpty(this.ICD_CODE))
                {
                    listVHisTreatmentFeeExpression.Add(o => o.ICD_CODE == this.ICD_CODE);
                }
                if (this.ICD_CODEs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => !String.IsNullOrWhiteSpace(o.ICD_CODE) && this.ICD_CODEs.Contains(o.ICD_CODE));
                }
                if (this.KSK_CONTRACT_ID.HasValue)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_KSK_CONTRACT_ID.HasValue && o.TDL_KSK_CONTRACT_ID.Value == this.KSK_CONTRACT_ID.Value);
                }
                if (this.KSK_CONTRACT_IDs != null)
                {
                    listVHisTreatmentFeeExpression.Add(o => o.TDL_KSK_CONTRACT_ID.HasValue && this.KSK_CONTRACT_IDs.Contains(o.TDL_KSK_CONTRACT_ID.Value));
                }

                search.listVHisTreatmentFeeExpression.AddRange(listVHisTreatmentFeeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisTreatmentFeeExpression.Clear();
                search.listVHisTreatmentFeeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
