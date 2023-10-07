using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMest
{
    public class HisExpMestView1FilterQuery : HisExpMestView1Filter
    {
        public HisExpMestView1FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_1, bool>>> listVHisExpMest1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_1, bool>>>();

        internal HisExpMestSO Query()
        {
            HisExpMestSO search = new HisExpMestSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMest1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMest1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMest1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisExpMest1Expression.Add(o => o.EXP_MEST_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_LOGINNAME.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_USERNAME.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.EXP_MEST_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.EXP_MEST_STT_IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.EXP_MEST_STT_IDs.Contains(o.EXP_MEST_STT_ID));
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.AGGR_EXP_MEST_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && o.AGGR_EXP_MEST_ID.Value == this.AGGR_EXP_MEST_ID.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.HAS_AGGR.HasValue && this.HAS_AGGR.Value)
                {
                    listVHisExpMest1Expression.Add(o => o.AGGR_EXP_MEST_ID.HasValue);
                }
                if (this.HAS_AGGR.HasValue && !this.HAS_AGGR.Value)
                {
                    listVHisExpMest1Expression.Add(o => !o.AGGR_EXP_MEST_ID.HasValue);
                }
                if (this.REQ_DEPARTMENT_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.REQ_DEPARTMENT_ID == this.REQ_DEPARTMENT_ID.Value);
                }
                if (this.REQ_ROOM_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.REQ_ROOM_ID == this.REQ_ROOM_ID.Value);
                }

                if (this.REQ_DEPARTMENT_IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID));
                }
                if (this.REQ_ROOM_IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisExpMest1Expression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }

                if (this.EXP_MEST_TYPE_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.EXP_MEST_TYPE_ID == this.EXP_MEST_TYPE_ID.Value);
                }
                if (this.EXP_MEST_STT_ID.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.EXP_MEST_STT_ID == this.EXP_MEST_STT_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.EXP_MEST_CODE__EXACT))
                {
                    listVHisExpMest1Expression.Add(o => o.EXP_MEST_CODE == this.EXP_MEST_CODE__EXACT);
                }
                if (this.FINISH_DATE_FROM.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.FINISH_DATE.Value >= this.FINISH_DATE_FROM.Value);
                }
                if (this.FINISH_DATE_TO.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.FINISH_DATE.Value <= this.FINISH_DATE_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.FINISH_TIME.Value >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    listVHisExpMest1Expression.Add(o => o.FINISH_TIME.Value <= this.FINISH_TIME_TO.Value);
                }

                search.listVHisExpMest1Expression.AddRange(listVHisExpMest1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMest1Expression.Clear();
                search.listVHisExpMest1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
