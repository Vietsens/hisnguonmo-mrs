using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeSub
{
    public class HisPatientTypeSubViewFilterQuery : HisPatientTypeSubViewFilter
    {
        public HisPatientTypeSubViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_SUB, bool>>> listVHisPatientTypeSubExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_SUB, bool>>>();

        

        internal HisPatientTypeSubSO Query()
        {
            HisPatientTypeSubSO search = new HisPatientTypeSubSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisPatientTypeSubExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisPatientTypeSubExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisPatientTypeSubExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisPatientTypeSubExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisPatientTypeSubExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisPatientTypeSubExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisPatientTypeSubExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_SUB_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PATIENT_TYPE_SUB_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisPatientTypeSubExpression.AddRange(listVHisPatientTypeSubExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisPatientTypeSubExpression.Clear();
                search.listVHisPatientTypeSubExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
