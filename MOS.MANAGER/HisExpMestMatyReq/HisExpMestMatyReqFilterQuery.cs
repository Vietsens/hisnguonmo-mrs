using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    public class HisExpMestMatyReqFilterQuery : HisExpMestMatyReqFilter
    {
        public HisExpMestMatyReqFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATY_REQ, bool>>> listHisExpMestMatyReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATY_REQ, bool>>>();

        

        internal HisExpMestMatyReqSO Query()
        {
            HisExpMestMatyReqSO search = new HisExpMestMatyReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisExpMestMatyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisExpMestMatyReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisExpMestMatyReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisExpMestMatyReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisExpMestMatyReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listHisExpMestMatyReqExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID));
                }
                if (this.EXP_MEST_ID.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listHisExpMestMatyReqExpression.Add(o => this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID));
                }
                if (this.TDL_MEDI_STOCK_ID.HasValue)
                {
                    listHisExpMestMatyReqExpression.Add(o => o.TDL_MEDI_STOCK_ID == this.TDL_MEDI_STOCK_ID.Value);
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listHisExpMestMatyReqExpression.Add(o => this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID));
                }

                search.listHisExpMestMatyReqExpression.AddRange(listHisExpMestMatyReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestMatyReqExpression.Clear();
                search.listHisExpMestMatyReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
