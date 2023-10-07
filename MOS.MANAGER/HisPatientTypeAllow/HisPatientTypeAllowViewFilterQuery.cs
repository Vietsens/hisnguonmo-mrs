using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAllow
{
    public class HisPatientTypeAllowViewFilterQuery : HisPatientTypeAllowViewFilter
    {
        public HisPatientTypeAllowViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALLOW, bool>>> listVHisPatientTypeAllowExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALLOW, bool>>>();

        

        internal HisPatientTypeAllowSO Query()
        {
            HisPatientTypeAllowSO search = new HisPatientTypeAllowSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisPatientTypeAllowExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisPatientTypeAllowExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_ALLOW_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_ALLOW_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisPatientTypeAllowExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }

                search.listVHisPatientTypeAllowExpression.AddRange(listVHisPatientTypeAllowExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPatientTypeAllowExpression.Clear();
                search.listHisPatientTypeAllowExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
