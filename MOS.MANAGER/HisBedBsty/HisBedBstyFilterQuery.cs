using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedBsty
{
    public class HisBedBstyFilterQuery : HisBedBstyFilter
    {
        public HisBedBstyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_BED_BSTY, bool>>> listHisBedBstyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED_BSTY, bool>>>();

        

        internal HisBedBstySO Query()
        {
            HisBedBstySO search = new HisBedBstySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisBedBstyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisBedBstyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisBedBstyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisBedBstyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisBedBstyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BED_ID.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.BED_ID == this.BED_ID.Value);
                }
                if (this.BED_IDs != null)
                {
                    listHisBedBstyExpression.Add(o => this.BED_IDs.Contains(o.BED_ID));
                }
                if (this.BED_SERVICE_TYPE_ID.HasValue)
                {
                    listHisBedBstyExpression.Add(o => o.BED_SERVICE_TYPE_ID == this.BED_SERVICE_TYPE_ID.Value);
                }
                if (this.BED_SERVICE_TYPE_IDs != null)
                {
                    listHisBedBstyExpression.Add(o => this.BED_SERVICE_TYPE_IDs.Contains(o.BED_SERVICE_TYPE_ID));
                }

                search.listHisBedBstyExpression.AddRange(listHisBedBstyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisBedBstyExpression.Clear();
                search.listHisBedBstyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
