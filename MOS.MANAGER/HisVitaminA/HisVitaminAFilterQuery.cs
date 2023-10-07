using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisVitaminA
{
    public class HisVitaminAFilterQuery : HisVitaminAFilter
    {
        public HisVitaminAFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_VITAMIN_A, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<HIS_VITAMIN_A, bool>>>();

        internal HisVitaminASO Query()
        {
            HisVitaminASO search = new HisVitaminASO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null && this.IDs.Count > 0)
                {
                    listExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.REQUEST_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.REQUEST_TIME >= this.REQUEST_TIME_FROM.Value);
                }
                if (this.REQUEST_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.REQUEST_TIME <= this.REQUEST_TIME_TO.Value);
                }

                if (this.EXECUTE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME >= this.EXECUTE_TIME_FROM.Value);
                }
                if (this.EXECUTE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME <= this.EXECUTE_TIME_TO.Value);
                }

                search.listHisVitaminAExpression.AddRange(listExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisVitaminAExpression.Clear();
                search.listHisVitaminAExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
