using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAllow
{
    public class HisPatientTypeAllowFilterQuery : HisPatientTypeAllowFilter
    {
        public HisPatientTypeAllowFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALLOW, bool>>> listHisPatientTypeAllowExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALLOW, bool>>>();

        

        internal HisPatientTypeAllowSO Query()
        {
            HisPatientTypeAllowSO search = new HisPatientTypeAllowSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisPatientTypeAllowExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPatientTypeAllowExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPatientTypeAllowExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPatientTypeAllowExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisPatientTypeAllowExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisPatientTypeAllowExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_ID__OR__PATIENT_TYPE_ALLOW_ID.HasValue)
                {
                    listHisPatientTypeAllowExpression.Add(o => (o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID__OR__PATIENT_TYPE_ALLOW_ID.Value || o.PATIENT_TYPE_ALLOW_ID == this.PATIENT_TYPE_ID__OR__PATIENT_TYPE_ALLOW_ID.Value));
                }
                
                search.listHisPatientTypeAllowExpression.AddRange(listHisPatientTypeAllowExpression);
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
