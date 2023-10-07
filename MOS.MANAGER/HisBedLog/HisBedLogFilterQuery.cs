using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedLog
{
    public class HisBedLogFilterQuery : HisBedLogFilter
    {
        public HisBedLogFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_BED_LOG, bool>>> listHisBedLogExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED_LOG, bool>>>();

        

        internal HisBedLogSO Query()
        {
            HisBedLogSO search = new HisBedLogSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisBedLogExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisBedLogExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisBedLogExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisBedLogExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisBedLogExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.TREATMENT_BED_ROOM_ID.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.TREATMENT_BED_ROOM_ID == this.TREATMENT_BED_ROOM_ID.Value);
                }
                if (this.TREATMENT_BED_ROOM_IDs != null)
                {
                    listHisBedLogExpression.Add(o => this.TREATMENT_BED_ROOM_IDs.Contains(o.TREATMENT_BED_ROOM_ID));
                }
                if (this.BED_ID.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.BED_ID == this.BED_ID.Value);
                }
                if (this.BED_IDs != null)
                {
                    listHisBedLogExpression.Add(o => this.BED_IDs.Contains(o.BED_ID));
                }
                if (this.START_TIME_FROM.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.START_TIME >= this.START_TIME_FROM.Value);
                }
                if (this.START_TIME_TO.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.START_TIME <= this.START_TIME_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME <= this.FINISH_TIME_TO.Value);
                }
                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listHisBedLogExpression.Add(o => o.SERVICE_REQ_ID.HasValue && o.SERVICE_REQ_ID.Value == this.SERVICE_REQ_ID.Value);
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listHisBedLogExpression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID.Value));
                }
                if (this.IS_FINISH.HasValue && this.IS_FINISH.Value)
                {
                    listHisBedLogExpression.Add(o => o.FINISH_TIME.HasValue);
                }
                if (this.IS_FINISH.HasValue && !this.IS_FINISH.Value)
                {
                    listHisBedLogExpression.Add(o => !o.FINISH_TIME.HasValue);
                }

                search.listHisBedLogExpression.AddRange(listHisBedLogExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisBedLogExpression.Clear();
                search.listHisBedLogExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
