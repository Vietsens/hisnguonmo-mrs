using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    public class HisPatientTypeAlterViewFilterQuery : HisPatientTypeAlterViewFilter
    {
        public HisPatientTypeAlterViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALTER, bool>>> listVHisPatientTypeAlterExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALTER, bool>>>();

        

        internal HisPatientTypeAlterSO Query()
        {
            HisPatientTypeAlterSO search = new HisPatientTypeAlterSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisPatientTypeAlterExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.LOG_TIME_TO.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.LOG_TIME <= this.LOG_TIME_TO.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    search.listVHisPatientTypeAlterExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.TDL_PATIENT_ID.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.TDL_PATIENT_ID == this.TDL_PATIENT_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    search.listVHisPatientTypeAlterExpression.Add(o => this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID));
                }
                if (this.TREATMENT_TYPE_IDs != null)
                {
                    search.listVHisPatientTypeAlterExpression.Add(o => this.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID));
                }
                if (this.TREATMENT_TYPE_ID.HasValue)
                {
                    listVHisPatientTypeAlterExpression.Add(o => o.TREATMENT_TYPE_ID == this.TREATMENT_TYPE_ID.Value);
                }
                search.listVHisPatientTypeAlterExpression.AddRange(listVHisPatientTypeAlterExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisPatientTypeAlterExpression.Clear();
                search.listVHisPatientTypeAlterExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
