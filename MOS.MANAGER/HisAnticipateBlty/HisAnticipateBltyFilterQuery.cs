using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateBlty
{
    public class HisAnticipateBltyFilterQuery : HisAnticipateBltyFilter
    {
        public HisAnticipateBltyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_BLTY, bool>>> listHisAnticipateBltyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_BLTY, bool>>>();

        

        internal HisAnticipateBltySO Query()
        {
            HisAnticipateBltySO search = new HisAnticipateBltySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisAnticipateBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAnticipateBltyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAnticipateBltyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAnticipateBltyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisAnticipateBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.ANTICIPATE_ID.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.ANTICIPATE_ID == this.ANTICIPATE_ID.Value);
                }
                if (this.ANTICIPATE_IDs != null)
                {
                    listHisAnticipateBltyExpression.Add(o => this.ANTICIPATE_IDs.Contains(o.ANTICIPATE_ID));
                }
                if (this.BLOOD_TYPE_ID.HasValue)
                {
                    listHisAnticipateBltyExpression.Add(o => o.BLOOD_TYPE_ID == this.BLOOD_TYPE_ID.Value);
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listHisAnticipateBltyExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }

                search.listHisAnticipateBltyExpression.AddRange(listHisAnticipateBltyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisAnticipateBltyExpression.Clear();
                search.listHisAnticipateBltyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
