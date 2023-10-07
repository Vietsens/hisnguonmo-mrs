using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    public class HisExpMestMetyReqView1FilterQuery : HisExpMestMetyReqView1Filter
    {
        public HisExpMestMetyReqView1FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ_1, bool>>> listVHisExpMestMetyReq1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ_1, bool>>>();



        internal HisExpMestMetyReqSO Query()
        {
            HisExpMestMetyReqSO search = new HisExpMestMetyReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EXP_MEST_ID.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.TDL_MEDI_STOCK_ID.HasValue)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => o.TDL_MEDI_STOCK_ID == this.TDL_MEDI_STOCK_ID.Value);
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMetyReq1Expression.Add(o => this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID));
                }

                search.listVHisExpMestMetyReq1Expression.AddRange(listVHisExpMestMetyReq1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMetyReq1Expression.Clear();
                search.listVHisExpMestMetyReq1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
