using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCacheMonitor
{
    public class HisCacheMonitorFilterQuery : HisCacheMonitorFilter
    {
        public HisCacheMonitorFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_CACHE_MONITOR, bool>>> listHisCacheMonitorExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CACHE_MONITOR, bool>>>();



        internal HisCacheMonitorSO Query()
        {
            HisCacheMonitorSO search = new HisCacheMonitorSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisCacheMonitorExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisCacheMonitorExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisCacheMonitorExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisCacheMonitorExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisCacheMonitorExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisCacheMonitorExpression.Add(o => this.IDs.Contains(o.ID));
                }

                if (!String.IsNullOrEmpty(this.DATA_NAME))
                {
                    listHisCacheMonitorExpression.Add(o => o.DATA_NAME == this.DATA_NAME);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisCacheMonitorExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DATA_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                #endregion

                search.listHisCacheMonitorExpression.AddRange(listHisCacheMonitorExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisCacheMonitorExpression.Clear();
                search.listHisCacheMonitorExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
