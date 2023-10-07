using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqMety
{
    public class HisServiceReqMetyFilterQuery : HisServiceReqMetyFilter
    {
        public HisServiceReqMetyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_METY, bool>>> listHisServiceReqMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_METY, bool>>>();



        internal HisServiceReqMetySO Query()
        {
            HisServiceReqMetySO search = new HisServiceReqMetySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisServiceReqMetyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisServiceReqMetyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisServiceReqMetyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisServiceReqMetyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisServiceReqMetyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion
                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => this.SERVICE_REQ_ID.Value == o.SERVICE_REQ_ID);
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listHisServiceReqMetyExpression.Add(o => this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listHisServiceReqMetyExpression.Add(o => o.MEDICINE_TYPE_ID.HasValue && this.MEDICINE_TYPE_ID.Value == o.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listHisServiceReqMetyExpression.Add(o => o.MEDICINE_TYPE_ID.HasValue && this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID.Value));
                }

                search.listHisServiceReqMetyExpression.AddRange(listHisServiceReqMetyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServiceReqMetyExpression.Clear();
                search.listHisServiceReqMetyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
