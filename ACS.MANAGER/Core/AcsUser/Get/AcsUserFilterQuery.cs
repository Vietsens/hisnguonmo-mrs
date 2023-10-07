using ACS.DAO.StagingObject;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace ACS.MANAGER.Core.AcsUser.Get
{
    public class AcsUserFilterQuery : AcsUserFilter
    {
        public AcsUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<ACS_USER, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<ACS_USER, bool>>>();

        internal OrderProcessorBase OrderProcess = new OrderProcessorBase();

        internal AcsUserSO Query()
        {
            AcsUserSO search = new AcsUserSO();
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

                if (!String.IsNullOrEmpty(this.LOGINNAME))
                {
                    listExpression.Add(o => o.LOGINNAME.ToLower() == this.LOGINNAME.ToLower());
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listExpression.Add(o => o.LOGINNAME.ToLower().Contains(this.KEY_WORD) || o.USERNAME.ToLower().Contains(this.KEY_WORD) || o.EMAIL.ToLower().Contains(this.KEY_WORD) || o.MOBILE.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.CN_WORD))
                {
                    this.CN_WORD = this.CN_WORD.ToLower().Trim();
                    listExpression.Add(o => o.LOGINNAME.ToLower().Contains(this.CN_WORD) || o.USERNAME.ToLower().Contains(this.CN_WORD));
                }
                #endregion

                search.listAcsUserExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<ACS_USER>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listAcsUserExpression.Clear();
                search.listAcsUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
