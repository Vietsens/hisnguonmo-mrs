using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public class HisImpMestMedicineFilterQuery : HisImpMestMedicineFilter
    {
        public HisImpMestMedicineFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MEDICINE, bool>>> listHisImpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MEDICINE, bool>>>();

        

        internal HisImpMestMedicineSO Query()
        {
            HisImpMestMedicineSO search = new HisImpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisImpMestMedicineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.IMP_MEST_ID.HasValue)
                {
                    search.listHisImpMestMedicineExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    search.listHisImpMestMedicineExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                #endregion

                if (this.MEDICINE_ID.HasValue)
                {
                    listHisImpMestMedicineExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    listHisImpMestMedicineExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.IMP_MEST_ID__NOT__EQUAL.HasValue)
                {
                    listHisImpMestMedicineExpression.Add(o => o.IMP_MEST_ID != this.IMP_MEST_ID__NOT__EQUAL.Value);
                }
                search.listHisImpMestMedicineExpression.AddRange(listHisImpMestMedicineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestMedicineExpression.Clear();
                search.listHisImpMestMedicineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
