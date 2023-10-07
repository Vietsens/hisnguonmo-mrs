using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipUser
{
    public class HisEkipUserFilterQuery : HisEkipUserFilter
    {
        public HisEkipUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EKIP_USER, bool>>> listHisEkipUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EKIP_USER, bool>>>();

        

        internal HisEkipUserSO Query()
        {
            HisEkipUserSO search = new HisEkipUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisEkipUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisEkipUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisEkipUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisEkipUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisEkipUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EKIP_ID.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.EKIP_ID == this.EKIP_ID.Value);
                }
                if (this.EKIP_IDs != null)
                {
                    listHisEkipUserExpression.Add(o => this.EKIP_IDs.Contains(o.EKIP_ID));
                }
                if (this.EXECUTE_ROLE_ID.HasValue)
                {
                    listHisEkipUserExpression.Add(o => o.EXECUTE_ROLE_ID == this.EXECUTE_ROLE_ID.Value);
                }
                if (this.EXECUTE_ROLE_IDs != null)
                {
                    listHisEkipUserExpression.Add(o => this.EXECUTE_ROLE_IDs.Contains(o.EXECUTE_ROLE_ID));
                }

                search.listHisEkipUserExpression.AddRange(listHisEkipUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisEkipUserExpression.Clear();
                search.listHisEkipUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
