using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientType
{
	public class HisPatientTypeFilterQuery : HisPatientTypeFilter
	{
		public HisPatientTypeFilterQuery()
			: base()
		{

		}

		internal List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE, bool>>> listHisPatientTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE, bool>>>();

		

		internal HisPatientTypeSO Query()
		{
			HisPatientTypeSO search = new HisPatientTypeSO();
			try
			{
				#region Abstract Base
				if (this.ID.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.ID == this.ID.Value);
				}
				if (this.IDs != null)
				{
					search.listHisPatientTypeExpression.Add(o => this.IDs.Contains(o.ID));
				}
				if (this.IS_ACTIVE.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
				}
				if (this.CREATE_TIME_FROM.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
				}
				if (this.CREATE_TIME_TO.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
				}
				if (this.MODIFY_TIME_FROM.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
				}
				if (this.MODIFY_TIME_TO.HasValue)
				{
					listHisPatientTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
				}
				if (!String.IsNullOrEmpty(this.CREATOR))
				{
					listHisPatientTypeExpression.Add(o => o.CREATOR == this.CREATOR);
				}
				if (!String.IsNullOrEmpty(this.MODIFIER))
				{
					listHisPatientTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
				}
				if (!String.IsNullOrEmpty(this.GROUP_CODE))
				{
					listHisPatientTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
				}
				#endregion

                if (this.IS_COPAYMENT.HasValue && this.IS_COPAYMENT.Value)
                {
                    listHisPatientTypeExpression.Add(o => o.IS_COPAYMENT.HasValue && o.IS_COPAYMENT.Value == 1);//can sua code
                }
                if (this.IS_COPAYMENT.HasValue && !this.IS_COPAYMENT.Value)
                {
                    listHisPatientTypeExpression.Add(o => !o.IS_COPAYMENT.HasValue || o.IS_COPAYMENT.Value != 1);//can sua code
                }
                if (!String.IsNullOrEmpty(this.PATIENT_TYPE_CODE__EXACT))
				{
                    listHisPatientTypeExpression.Add(o => o.PATIENT_TYPE_CODE == this.PATIENT_TYPE_CODE__EXACT);
				}
				if (!String.IsNullOrEmpty(this.KEY_WORD))
				{
					this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
					listHisPatientTypeExpression.Add(o =>
						o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
						o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
						o.PATIENT_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
						o.PATIENT_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
						o.DESCRIPTION.ToLower().Contains(this.KEY_WORD)
						);
				}

				search.listHisPatientTypeExpression.AddRange(listHisPatientTypeExpression);
				search.OrderField = ORDER_FIELD;
				search.OrderDirection = ORDER_DIRECTION;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				search.listHisPatientTypeExpression.Clear();
				search.listHisPatientTypeExpression.Add(o => o.ID == NEGATIVE_ID);
			}
			return search;
		}
	}
}
