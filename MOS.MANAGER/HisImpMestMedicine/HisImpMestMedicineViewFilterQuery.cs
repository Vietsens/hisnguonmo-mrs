using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public class HisImpMestMedicineViewFilterQuery : HisImpMestMedicineViewFilter
    {
        public HisImpMestMedicineViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE, bool>>> listVHisImpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE, bool>>>();



        internal HisImpMestMedicineSO Query()
        {
            HisImpMestMedicineSO search = new HisImpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestMedicineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestMedicineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IMP_TIME >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IMP_TIME <= this.IMP_TIME_TO.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.HAS_MEDI_STOCK_PERIOD.HasValue)
                {
                    if (this.HAS_MEDI_STOCK_PERIOD.Value)
                    {
                        listVHisImpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID != null);
                    }
                    else
                    {
                        listVHisImpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == null);
                    }
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.IMP_MEST_STT_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IMP_MEST_STT_ID == this.IMP_MEST_STT_ID.Value);
                }
                if (this.IMP_MEST_TYPE_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.IMP_MEST_TYPE_ID == this.IMP_MEST_TYPE_ID.Value);
                }
                if (this.AGGR_IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.AGGR_IMP_MEST_ID.HasValue && o.AGGR_IMP_MEST_ID.Value == this.AGGR_IMP_MEST_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisImpMestMedicineExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(s => MEDICINE_TYPE_IDs.Contains(s.MEDICINE_TYPE_ID));
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(s => this.SERVICE_IDs.Contains(s.SERVICE_ID));
                }
                if (this.IMP_MEST_STT_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(s => this.IMP_MEST_STT_IDs.Contains(s.IMP_MEST_STT_ID));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisImpMestMedicineExpression.Add(s => this.MEDI_STOCK_IDs.Contains(s.MEDI_STOCK_ID));
                }

                search.listVHisImpMestMedicineExpression.AddRange(listVHisImpMestMedicineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestMedicineExpression.Clear();
                search.listVHisImpMestMedicineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
