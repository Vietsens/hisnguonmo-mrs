using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareDetail
{
	public class HisCareDetailFilterQuery : HisCareDetailFilter
	{
		public HisCareDetailFilterQuery()
			: base()
		{

		}

		internal List<System.Linq.Expressions.Expression<Func<HIS_CARE_DETAIL, bool>>> listHisCareDetailExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARE_DETAIL, bool>>>();

		

		internal HisCareDetailSO Query()
		{
			HisCareDetailSO search = new HisCareDetailSO();
			try
			{
				#region Abstract Base
				if (this.ID.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.ID == this.ID.Value);
				}
				if (this.IDs != null)
				{
					listHisCareDetailExpression.Add(o => this.IDs.Contains(o.ID));
				}
				if (this.IS_ACTIVE.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
				}
				if (this.CREATE_TIME_FROM.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
				}
				if (this.CREATE_TIME_TO.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
				}
				if (this.MODIFY_TIME_FROM.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
				}
				if (this.MODIFY_TIME_TO.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
				}
				if (!String.IsNullOrEmpty(this.CREATOR))
				{
					listHisCareDetailExpression.Add(o => o.CREATOR == this.CREATOR);
				}
				if (!String.IsNullOrEmpty(this.MODIFIER))
				{
					listHisCareDetailExpression.Add(o => o.MODIFIER == this.MODIFIER);
				}
				if (!String.IsNullOrEmpty(this.GROUP_CODE))
				{
					listHisCareDetailExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
				}
				#endregion

				if (this.CARE_ID.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.CARE_ID == this.CARE_ID.Value);
				}
				if (this.CARE_TYPE_ID.HasValue)
				{
					listHisCareDetailExpression.Add(o => o.CARE_TYPE_ID == this.CARE_TYPE_ID.Value);
				}

				search.listHisCareDetailExpression.AddRange(listHisCareDetailExpression);
				search.OrderField = ORDER_FIELD;
				search.OrderDirection = ORDER_DIRECTION;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				search.listHisCareDetailExpression.Clear();
				search.listHisCareDetailExpression.Add(o => o.ID == NEGATIVE_ID);
			}
			return search;
		}
	}
}
