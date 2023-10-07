using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestTypeUser
{
    public class HisImpMestTypeUserFilterQuery : HisImpMestTypeUserFilter
    {
        public HisImpMestTypeUserFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE_USER, bool>>> listHisImpMestTypeUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE_USER, bool>>>();

        

        internal HisImpMestTypeUserSO Query()
        {
            HisImpMestTypeUserSO search = new HisImpMestTypeUserSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisImpMestTypeUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisImpMestTypeUserExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisImpMestTypeUserExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisImpMestTypeUserExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisImpMestTypeUserExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.IMP_MEST_TYPE_ID.HasValue)
                {
                    listHisImpMestTypeUserExpression.Add(o => o.IMP_MEST_TYPE_ID == this.IMP_MEST_TYPE_ID.Value);
                }
                if (this.IMP_MEST_TYPE_IDs != null)
                {
                    listHisImpMestTypeUserExpression.Add(o => this.IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID));
                }
                if (!string.IsNullOrWhiteSpace(this.LOGINNAME))
                {
                    listHisImpMestTypeUserExpression.Add(o => o.LOGINNAME == this.LOGINNAME);
                }
                if (this.LOGINNAMEs != null)
                {
                    listHisImpMestTypeUserExpression.Add(o => this.LOGINNAMEs.Contains(o.LOGINNAME));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisImpMestTypeUserExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.LOGINNAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisImpMestTypeUserExpression.AddRange(listHisImpMestTypeUserExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestTypeUserExpression.Clear();
                search.listHisImpMestTypeUserExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
