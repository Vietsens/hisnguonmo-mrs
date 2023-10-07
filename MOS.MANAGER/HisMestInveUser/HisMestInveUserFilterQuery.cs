using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestInveUser
{
    public class HisMestInveUserFilterQuery : HisMestInveUserFilter
    {
        public HisMestInveUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVE_USER, bool>>> listHisMestInveUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVE_USER, bool>>>();



        internal HisMestInveUserSO Query()
        {
            HisMestInveUserSO search = new HisMestInveUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisMestInveUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMestInveUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMestInveUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMestInveUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisMestInveUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MEST_INVENTORY_ID.HasValue)
                {
                    listHisMestInveUserExpression.Add(o => o.MEST_INVENTORY_ID == this.MEST_INVENTORY_ID.Value);
                }
                if (this.MEST_INVENTORY_IDs != null)
                {
                    listHisMestInveUserExpression.Add(o => this.MEST_INVENTORY_IDs.Contains(o.MEST_INVENTORY_ID));
                }

                search.listHisMestInveUserExpression.AddRange(listHisMestInveUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMestInveUserExpression.Clear();
                search.listHisMestInveUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
