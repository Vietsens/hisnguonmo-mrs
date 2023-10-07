using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    public class HisExpMestBltyReqFilterQuery : HisExpMestBltyReqFilter
    {
        public HisExpMestBltyReqFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLTY_REQ, bool>>> listHisExpMestBltyReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLTY_REQ, bool>>>();

        

        internal HisExpMestBltyReqSO Query()
        {
            HisExpMestBltyReqSO search = new HisExpMestBltyReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisExpMestBltyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisExpMestBltyReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisExpMestBltyReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisExpMestBltyReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisExpMestBltyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EXP_MEST_ID.HasValue)
                {
                    listHisExpMestBltyReqExpression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listHisExpMestBltyReqExpression.Add(o => this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID));
                }
                
                search.listHisExpMestBltyReqExpression.AddRange(listHisExpMestBltyReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestBltyReqExpression.Clear();
                search.listHisExpMestBltyReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
