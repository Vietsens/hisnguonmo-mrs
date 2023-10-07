using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestBlood
{
    public class HisImpMestBloodViewFilterQuery : HisImpMestBloodViewFilter
    {
        public HisImpMestBloodViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_BLOOD, bool>>> listVHisImpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_BLOOD, bool>>>();



        internal HisImpMestBloodSO Query()
        {
            HisImpMestBloodSO search = new HisImpMestBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.BLOOD_ID.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.BLOOD_ID == this.BLOOD_ID.Value);
                }
                if (this.BLOOD_IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.BLOOD_IDs.Contains(o.BLOOD_ID));
                }
                if (this.IMP_MEST_STT_ID.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.IMP_MEST_STT_ID == this.IMP_MEST_STT_ID.Value);
                }
                if (this.IMP_MEST_STT_IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.IMP_MEST_STT_IDs.Contains(o.IMP_MEST_STT_ID));
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisImpMestBloodExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisImpMestBloodExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }

                search.listVHisImpMestBloodExpression.AddRange(listVHisImpMestBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestBloodExpression.Clear();
                search.listVHisImpMestBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
