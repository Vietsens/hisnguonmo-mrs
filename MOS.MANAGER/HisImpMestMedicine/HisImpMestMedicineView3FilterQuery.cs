using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public class HisImpMestMedicineView3FilterQuery : HisImpMestMedicineView3Filter
    {
        public HisImpMestMedicineView3FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_3, bool>>> listVHisImpMestMedicine3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_3, bool>>>();



        internal HisImpMestMedicineSO Query()
        {
            HisImpMestMedicineSO search = new HisImpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMedicine3Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestMedicine3Expression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisImpMestMedicine3Expression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisImpMestMedicine3Expression.Add(s => MEDICINE_TYPE_IDs.Contains(s.MEDICINE_TYPE_ID));
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicine3Expression.Add(o => o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }

                search.listVHisImpMestMedicine3Expression.AddRange(listVHisImpMestMedicine3Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestMedicine3Expression.Clear();
                search.listVHisImpMestMedicine3Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
