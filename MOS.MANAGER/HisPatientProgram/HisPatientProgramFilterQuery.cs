using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientProgram
{
    public class HisPatientProgramFilterQuery : HisPatientProgramFilter
    {
        public HisPatientProgramFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_PROGRAM, bool>>> listHisPatientProgramExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_PROGRAM, bool>>>();

        

        internal HisPatientProgramSO Query()
        {
            HisPatientProgramSO search = new HisPatientProgramSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisPatientProgramExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPatientProgramExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPatientProgramExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPatientProgramExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisPatientProgramExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.PATIENT_ID.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.PATIENT_ID == this.PATIENT_ID.Value);
                }
                if (this.PATIENT_IDs != null)
                {
                    listHisPatientProgramExpression.Add(o => this.PATIENT_IDs.Contains(o.PATIENT_ID));
                }
                if (this.PROGRAM_ID.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.PROGRAM_ID == this.PROGRAM_ID.Value);
                }
                if (this.PROGRAM_IDs != null)
                {
                    listHisPatientProgramExpression.Add(o => this.PROGRAM_IDs.Contains(o.PROGRAM_ID));
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    listHisPatientProgramExpression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }

                search.listHisPatientProgramExpression.AddRange(listHisPatientProgramExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPatientProgramExpression.Clear();
                search.listHisPatientProgramExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
