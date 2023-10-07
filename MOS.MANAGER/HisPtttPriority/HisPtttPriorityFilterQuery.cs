using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttPriority
{
    public class HisPtttPriorityFilterQuery : HisPtttPriorityFilter
    {
        public HisPtttPriorityFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PTTT_PRIORITY, bool>>> listHisPtttPriorityExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PTTT_PRIORITY, bool>>>();

        

        internal HisPtttPrioritySO Query()
        {
            HisPtttPrioritySO search = new HisPtttPrioritySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisPtttPriorityExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPtttPriorityExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPtttPriorityExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPtttPriorityExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPtttPriorityExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.PTTT_PRIORITY_CODE__EXACT))
                {
                    listHisPtttPriorityExpression.Add(o => o.PTTT_PRIORITY_CODE == this.PTTT_PRIORITY_CODE__EXACT);
                }
                
                search.listHisPtttPriorityExpression.AddRange(listHisPtttPriorityExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPtttPriorityExpression.Clear();
                search.listHisPtttPriorityExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
