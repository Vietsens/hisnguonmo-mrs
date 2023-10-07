using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamSereDire
{
    public class HisExamSereDireFilterQuery : HisExamSereDireFilter
    {
        public HisExamSereDireFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERE_DIRE, bool>>> listHisExamSereDireExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERE_DIRE, bool>>>();

        

        internal HisExamSereDireSO Query()
        {
            HisExamSereDireSO search = new HisExamSereDireSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisExamSereDireExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisExamSereDireExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisExamSereDireExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisExamSereDireExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.SERVICE_REQ_ID == this.SERVICE_REQ_ID.Value);
                }
                if (this.DISEASE_RELATION_ID.HasValue)
                {
                    listHisExamSereDireExpression.Add(o => o.DISEASE_RELATION_ID == this.DISEASE_RELATION_ID.Value);
                }

                search.listHisExamSereDireExpression.AddRange(listHisExamSereDireExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExamSereDireExpression.Clear();
                search.listHisExamSereDireExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
