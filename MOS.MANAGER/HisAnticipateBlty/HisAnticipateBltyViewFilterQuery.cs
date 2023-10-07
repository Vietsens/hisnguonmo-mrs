using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateBlty
{
    public class HisAnticipateBltyViewFilterQuery : HisAnticipateBltyViewFilter
    {
        public HisAnticipateBltyViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_BLTY, bool>>> listVHisAnticipateBltyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_BLTY, bool>>>();

        

        internal HisAnticipateBltySO Query()
        {
            HisAnticipateBltySO search = new HisAnticipateBltySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisAnticipateBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisAnticipateBltyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisAnticipateBltyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisAnticipateBltyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisAnticipateBltyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.ANTICIPATE_ID.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.ANTICIPATE_ID == this.ANTICIPATE_ID.Value);
                }
                if (this.ANTICIPATE_IDs != null)
                {
                    listVHisAnticipateBltyExpression.Add(o => this.ANTICIPATE_IDs.Contains(o.ANTICIPATE_ID));
                }
                if (this.BLOOD_TYPE_ID.HasValue)
                {
                    listVHisAnticipateBltyExpression.Add(o => o.BLOOD_TYPE_ID == this.BLOOD_TYPE_ID.Value);
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listVHisAnticipateBltyExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAnticipateBltyExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisAnticipateBltyExpression.AddRange(listVHisAnticipateBltyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisAnticipateBltyExpression.Clear();
                search.listVHisAnticipateBltyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
