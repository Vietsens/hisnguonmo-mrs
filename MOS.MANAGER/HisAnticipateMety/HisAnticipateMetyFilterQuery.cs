using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMety
{
    public class HisAnticipateMetyFilterQuery : HisAnticipateMetyFilter
    {
        public HisAnticipateMetyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_METY, bool>>> listHisAnticipateMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_METY, bool>>>();

        

        internal HisAnticipateMetySO Query()
        {
            HisAnticipateMetySO search = new HisAnticipateMetySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisAnticipateMetyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAnticipateMetyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAnticipateMetyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAnticipateMetyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.ANTICIPATE_ID.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.ANTICIPATE_ID == this.ANTICIPATE_ID.Value);
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listHisAnticipateMetyExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.ANTICIPATE_IDs != null)
                {
                    listHisAnticipateMetyExpression.Add(o => this.ANTICIPATE_IDs.Contains(o.ANTICIPATE_ID));
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listHisAnticipateMetyExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }

                search.listHisAnticipateMetyExpression.AddRange(listHisAnticipateMetyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisAnticipateMetyExpression.Clear();
                search.listHisAnticipateMetyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
