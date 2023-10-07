using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoleUser
{
    public class HisExecuteRoleUserFilterQuery : HisExecuteRoleUserFilter
    {
        public HisExecuteRoleUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE_USER, bool>>> listHisExecuteRoleUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROLE_USER, bool>>>();

        

        internal HisExecuteRoleUserSO Query()
        {
            HisExecuteRoleUserSO search = new HisExecuteRoleUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisExecuteRoleUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisExecuteRoleUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisExecuteRoleUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisExecuteRoleUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXECUTE_ROLE_ID.HasValue)
                {
                    listHisExecuteRoleUserExpression.Add(o => o.EXECUTE_ROLE_ID == this.EXECUTE_ROLE_ID.Value);
                }
                if (this.EXECUTE_ROLE_IDs != null)
                {
                    listHisExecuteRoleUserExpression.Add(o => this.EXECUTE_ROLE_IDs.Contains(o.EXECUTE_ROLE_ID));
                }
                if (!String.IsNullOrEmpty(this.LOGINNAME__EXACT))
                {
                    listHisExecuteRoleUserExpression.Add(o => o.LOGINNAME == this.LOGINNAME__EXACT);
                }

                search.listHisExecuteRoleUserExpression.AddRange(listHisExecuteRoleUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExecuteRoleUserExpression.Clear();
                search.listHisExecuteRoleUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
