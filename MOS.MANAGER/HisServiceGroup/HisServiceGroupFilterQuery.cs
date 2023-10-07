using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceGroup
{
    public class HisServiceGroupFilterQuery : HisServiceGroupFilter
    {
        public HisServiceGroupFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_GROUP, bool>>> listHisServiceGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_GROUP, bool>>>();

        

        internal HisServiceGroupSO Query()
        {
            HisServiceGroupSO search = new HisServiceGroupSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisServiceGroupExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisServiceGroupExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisServiceGroupExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisServiceGroupExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisServiceGroupExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.CAN_VIEW.HasValue && this.CAN_VIEW.Value)
                {
                    string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();

                    listHisServiceGroupExpression.Add(o => o.CREATOR == loginName || o.IS_PUBLIC.HasValue && o.IS_PUBLIC.Value == ManagerConstant.IS_TRUE);
                }
                if (this.CAN_VIEW.HasValue && !this.CAN_VIEW.Value)
                {
                    string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();

                    listHisServiceGroupExpression.Add(o => o.CREATOR != loginName && (!o.IS_PUBLIC.HasValue || o.IS_PUBLIC.Value == ManagerConstant.IS_TRUE));
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisServiceGroupExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_GROUP_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_GROUP_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisServiceGroupExpression.AddRange(listHisServiceGroupExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServiceGroupExpression.Clear();
                search.listHisServiceGroupExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
