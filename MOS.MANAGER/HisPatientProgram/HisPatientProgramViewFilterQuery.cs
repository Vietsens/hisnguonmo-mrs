using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientProgram
{
	public class HisPatientProgramViewFilterQuery : HisPatientProgramViewFilter
	{
		public HisPatientProgramViewFilterQuery()
			: base()
		{

		}

		internal List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_PROGRAM, bool>>> listVHisPatientProgramExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_PROGRAM, bool>>>();

		

		internal HisPatientProgramSO Query()
		{
			HisPatientProgramSO search = new HisPatientProgramSO();
			try
			{
				#region Abstract Base
				if (this.ID.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.ID == this.ID.Value);
				}
				if (this.IDs != null)
				{
					listVHisPatientProgramExpression.Add(o => this.IDs.Contains(o.ID));
				}
				if (this.IS_ACTIVE.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
				}
				if (this.CREATE_TIME_FROM.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
				}
				if (this.CREATE_TIME_TO.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
				}
				if (this.MODIFY_TIME_FROM.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
				}
				if (this.MODIFY_TIME_TO.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
				}
				if (!String.IsNullOrEmpty(this.CREATOR))
				{
					listVHisPatientProgramExpression.Add(o => o.CREATOR == this.CREATOR);
				}
				if (!String.IsNullOrEmpty(this.MODIFIER))
				{
					listVHisPatientProgramExpression.Add(o => o.MODIFIER == this.MODIFIER);
				}
				if (!String.IsNullOrEmpty(this.GROUP_CODE))
				{
					listVHisPatientProgramExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
				}
				if (this.IDs != null)
				{
					listVHisPatientProgramExpression.Add(o => this.IDs.Contains(o.ID));
				}
				#endregion

				if (this.PATIENT_ID.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.PATIENT_ID == this.PATIENT_ID.Value);
				}
				if (this.PATIENT_IDs != null)
				{
					listVHisPatientProgramExpression.Add(o => this.PATIENT_IDs.Contains(o.PATIENT_ID));
				}
				if (this.PROGRAM_ID.HasValue)
				{
					listVHisPatientProgramExpression.Add(o => o.PROGRAM_ID == this.PROGRAM_ID.Value);
				}
				if (this.PROGRAM_IDs != null)
				{
					listVHisPatientProgramExpression.Add(o => this.PROGRAM_IDs.Contains(o.PROGRAM_ID));
				}
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisPatientProgramExpression.Add(o => o.PATIENT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.VIR_PATIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PHONE.ToLower().Contains(this.KEY_WORD)
                        || o.PROVINCE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.COMMUNE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.ADDRESS.ToLower().Contains(this.KEY_WORD)
                        || o.DISTRICT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PROGRAM_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.PROGRAM_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PATIENT_PROGRAM_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }

				search.listVHisPatientProgramExpression.AddRange(listVHisPatientProgramExpression);
				search.OrderField = ORDER_FIELD;
				search.OrderDirection = ORDER_DIRECTION;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				search.listVHisPatientProgramExpression.Clear();
				search.listVHisPatientProgramExpression.Add(o => o.ID == NEGATIVE_ID);
			}
			return search;
		}
	}
}
