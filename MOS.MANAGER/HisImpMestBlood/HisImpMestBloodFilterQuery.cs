using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestBlood
{
    public class HisImpMestBloodFilterQuery : HisImpMestBloodFilter
    {
        public HisImpMestBloodFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_BLOOD, bool>>> listHisImpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_BLOOD, bool>>>();

        

        internal HisImpMestBloodSO Query()
        {
            HisImpMestBloodSO search = new HisImpMestBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisImpMestBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisImpMestBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisImpMestBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisImpMestBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisImpMestBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listHisImpMestBloodExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.BLOOD_ID.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.BLOOD_ID == this.BLOOD_ID.Value);
                }
                if (this.BLOOD_IDs != null)
                {
                    listHisImpMestBloodExpression.Add(o => this.BLOOD_IDs.Contains(o.BLOOD_ID));
                }
                if (this.IMP_MEST_ID__NOT__EQUAL.HasValue)
                {
                    listHisImpMestBloodExpression.Add(o => o.IMP_MEST_ID != this.IMP_MEST_ID__NOT__EQUAL.Value);
                }

                search.listHisImpMestBloodExpression.AddRange(listHisImpMestBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestBloodExpression.Clear();
                search.listHisImpMestBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
