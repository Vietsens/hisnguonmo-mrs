using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestStt
{
    public class HisExpMestSttFilterQuery : HisExpMestSttFilter
    {
        public HisExpMestSttFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_STT, bool>>> listHisExpMestSttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_STT, bool>>>();

        

        internal HisExpMestSttSO Query()
        {
            HisExpMestSttSO search = new HisExpMestSttSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisExpMestSttExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisExpMestSttExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisExpMestSttExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisExpMestSttExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisExpMestSttExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.EXP_MEST_STT_CODE))
                {
                    search.listHisExpMestSttExpression.Add(o => o.EXP_MEST_STT_CODE == this.EXP_MEST_STT_CODE);
                }

                search.listHisExpMestSttExpression.AddRange(listHisExpMestSttExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestSttExpression.Clear();
                search.listHisExpMestSttExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
