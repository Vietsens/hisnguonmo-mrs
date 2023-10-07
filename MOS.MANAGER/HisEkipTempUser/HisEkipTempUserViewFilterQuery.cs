using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTempUser
{
    public class HisEkipTempUserViewFilterQuery : HisEkipTempUserViewFilter
    {
        public HisEkipTempUserViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_TEMP_USER, bool>>> listVHisEkipTempUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_TEMP_USER, bool>>>();

        

        internal HisEkipTempUserSO Query()
        {
            HisEkipTempUserSO search = new HisEkipTempUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisEkipTempUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisEkipTempUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisEkipTempUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisEkipTempUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EKIP_TEMP_ID != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.EKIP_TEMP_ID.Value == o.EKIP_TEMP_ID);
                }
                if (this.EXECUTE_ROLE_ID != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.EXECUTE_ROLE_ID.Value == o.EXECUTE_ROLE_ID);
                }

                if (this.EKIP_TEMP_IDs != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.EKIP_TEMP_IDs.Contains(o.EKIP_TEMP_ID));
                }
                if (this.EXECUTE_ROLE_IDs != null)
                {
                    listVHisEkipTempUserExpression.Add(o => this.EXECUTE_ROLE_IDs.Contains(o.EXECUTE_ROLE_ID));
                }

                search.listVHisEkipTempUserExpression.AddRange(listVHisEkipTempUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisEkipTempUserExpression.Clear();
                search.listVHisEkipTempUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
