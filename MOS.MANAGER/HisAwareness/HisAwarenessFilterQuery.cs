using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAwareness
{
	public class HisAwarenessFilterQuery : HisAwarenessFilter
	{
		public HisAwarenessFilterQuery()
			: base()
		{

		}

		internal List<System.Linq.Expressions.Expression<Func<HIS_AWARENESS, bool>>> listHisAwarenessExpression = new List<System.Linq.Expressions.Expression<Func<HIS_AWARENESS, bool>>>();

		

		internal HisAwarenessSO Query()
		{
			HisAwarenessSO search = new HisAwarenessSO();
			try
			{
				#region Abstract Base
				if (this.ID.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.ID == this.ID.Value);
				}
				if (this.IDs != null)
				{
					listHisAwarenessExpression.Add(o => this.IDs.Contains(o.ID));
				}
				if (this.IS_ACTIVE.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
				}
				if (this.CREATE_TIME_FROM.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
				}
				if (this.CREATE_TIME_TO.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
				}
				if (this.MODIFY_TIME_FROM.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
				}
				if (this.MODIFY_TIME_TO.HasValue)
				{
					listHisAwarenessExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
				}
				if (!String.IsNullOrEmpty(this.CREATOR))
				{
					listHisAwarenessExpression.Add(o => o.CREATOR == this.CREATOR);
				}
				if (!String.IsNullOrEmpty(this.MODIFIER))
				{
					listHisAwarenessExpression.Add(o => o.MODIFIER == this.MODIFIER);
				}
				if (!String.IsNullOrEmpty(this.GROUP_CODE))
				{
					listHisAwarenessExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
				}
				#endregion

				if (!String.IsNullOrEmpty(this.KEY_WORD))
				{
					this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
					listHisAwarenessExpression.Add(o =>
						o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
						o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
						o.AWARENESS_CODE.ToLower().Contains(this.KEY_WORD) ||
						o.AWARENESS_NAME.ToLower().Contains(this.KEY_WORD)
						);
				}

				search.listHisAwarenessExpression.AddRange(listHisAwarenessExpression);
				search.OrderField = ORDER_FIELD;
				search.OrderDirection = ORDER_DIRECTION;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				search.listHisAwarenessExpression.Clear();
				search.listHisAwarenessExpression.Add(o => o.ID == NEGATIVE_ID);
			}
			return search;
		}
	}
}
