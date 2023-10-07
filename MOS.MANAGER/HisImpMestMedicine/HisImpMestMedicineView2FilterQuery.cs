using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public class HisImpMestMedicineView2FilterQuery : HisImpMestMedicineView2Filter
    {
        public HisImpMestMedicineView2FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_2, bool>>> listVHisImpMestMedicine2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_2, bool>>>();



        internal HisImpMestMedicineSO Query()
        {
            HisImpMestMedicineSO search = new HisImpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMedicine2Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestMedicine2Expression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisImpMestMedicine2Expression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisImpMestMedicine2Expression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisImpMestMedicine2Expression.Add(s => MEDICINE_TYPE_IDs.Contains(s.MEDICINE_TYPE_ID));
                }

                search.listVHisImpMestMedicine2Expression.AddRange(listVHisImpMestMedicine2Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestMedicine2Expression.Clear();
                search.listVHisImpMestMedicine2Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
