using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    public class HisExpMestMatyReqViewFilterQuery : HisExpMestMatyReqViewFilter
    {
        public HisExpMestMatyReqViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATY_REQ, bool>>> listVHisExpMestMatyReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATY_REQ, bool>>>();

        

        internal HisExpMestMatyReqSO Query()
        {
            HisExpMestMatyReqSO search = new HisExpMestMatyReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EXP_MEST_ID.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID));
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID));
                }
                if (this.TDL_MEDI_STOCK_ID.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.TDL_MEDI_STOCK_ID == this.TDL_MEDI_STOCK_ID.Value);
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID));
                }
                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.SERVICE_REQ_ID.HasValue && o.SERVICE_REQ_ID.Value == this.SERVICE_REQ_ID.Value);
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listVHisExpMestMatyReqExpression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID.Value));
                }

                search.listVHisExpMestMatyReqExpression.AddRange(listVHisExpMestMatyReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMatyReqExpression.Clear();
                search.listVHisExpMestMatyReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
