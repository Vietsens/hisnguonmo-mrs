using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestStt
{
    public class HisImpMestSttFilterQuery : HisImpMestSttFilter
    {
        public HisImpMestSttFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_STT, bool>>> listHisImpMestSttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_STT, bool>>>();

        

        internal HisImpMestSttSO Query()
        {
            HisImpMestSttSO search = new HisImpMestSttSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisImpMestSttExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisImpMestSttExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisImpMestSttExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisImpMestSttExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisImpMestSttExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.IMP_MEST_STT_CODE))
                {
                    search.listHisImpMestSttExpression.Add(o => o.IMP_MEST_STT_CODE == this.IMP_MEST_STT_CODE);
                }
                search.listHisImpMestSttExpression.AddRange(listHisImpMestSttExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestSttExpression.Clear();
                search.listHisImpMestSttExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
