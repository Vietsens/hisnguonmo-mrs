using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestUser
{
    public class HisImpMestUserFilterQuery : HisImpMestUserFilter
    {
        public HisImpMestUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_USER, bool>>> listHisImpMestUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_USER, bool>>>();

        

        internal HisImpMestUserSO Query()
        {
            HisImpMestUserSO search = new HisImpMestUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisImpMestUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisImpMestUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisImpMestUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisImpMestUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisImpMestUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listHisImpMestUserExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listHisImpMestUserExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                search.listHisImpMestUserExpression.AddRange(listHisImpMestUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestUserExpression.Clear();
                search.listHisImpMestUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
