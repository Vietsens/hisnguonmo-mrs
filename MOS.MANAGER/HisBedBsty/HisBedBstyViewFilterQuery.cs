using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedBsty
{
    public class HisBedBstyViewFilterQuery : HisBedBstyViewFilter
    {
        public HisBedBstyViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BED_BSTY, bool>>> listVHisBedBstyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BED_BSTY, bool>>>();

        

        internal HisBedBstySO Query()
        {
            HisBedBstySO search = new HisBedBstySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisBedBstyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBedBstyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBedBstyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBedBstyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBedBstyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BED_ID.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.BED_ID == this.BED_ID.Value);
                }
                if (this.BED_IDs != null)
                {
                    listVHisBedBstyExpression.Add(o => this.BED_IDs.Contains(o.BED_ID));
                }
                if (this.ROOM_ID.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.ROOM_IDs != null)
                {
                    listVHisBedBstyExpression.Add(o => this.ROOM_IDs.Contains(o.ROOM_ID));
                }
                if (this.BED_SERVICE_TYPE_ID.HasValue)
                {
                    listVHisBedBstyExpression.Add(o => o.BED_SERVICE_TYPE_ID == this.BED_SERVICE_TYPE_ID.Value);
                }
                if (this.BED_SERVICE_TYPE_IDs != null)
                {
                    listVHisBedBstyExpression.Add(o => this.BED_SERVICE_TYPE_IDs.Contains(o.BED_SERVICE_TYPE_ID));
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisBedBstyExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisBedBstyExpression.AddRange(listVHisBedBstyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBedBstyExpression.Clear();
                search.listVHisBedBstyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
