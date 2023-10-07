using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMedicine
{
    public class HisExpMestMedicineFilterQuery : HisExpMestMedicineFilter
    {
        public HisExpMestMedicineFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MEDICINE, bool>>> listHisExpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MEDICINE, bool>>>();



        internal HisExpMestMedicineSO Query()
        {
            HisExpMestMedicineSO search = new HisExpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisExpMestMedicineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisExpMestMedicineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXP_MEST_ID != null)
                {
                    listHisExpMestMedicineExpression.Add(o => this.EXP_MEST_ID.Value == o.EXP_MEST_ID);
                }
                if (this.MEDICINE_ID != null)
                {
                    listHisExpMestMedicineExpression.Add(o => this.MEDICINE_ID.Value == o.MEDICINE_ID);
                }
                if (this.TDL_MEDI_STOCK_ID != null)
                {
                    listHisExpMestMedicineExpression.Add(o => this.TDL_MEDI_STOCK_ID.Value == o.TDL_MEDI_STOCK_ID);
                }
                if (this.TDL_MEDICINE_TYPE_ID != null)
                {
                    listHisExpMestMedicineExpression.Add(o => this.TDL_MEDICINE_TYPE_ID.Value == o.TDL_MEDICINE_TYPE_ID);
                }

                if (this.EXP_MEST_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.MEDICINE_ID != null && this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.TDL_MEDI_STOCK_ID != null && this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID.Value));
                }
                if (this.TDL_MEDICINE_TYPE_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.TDL_MEDICINE_TYPE_ID != null && this.TDL_MEDICINE_TYPE_IDs.Contains(o.TDL_MEDICINE_TYPE_ID.Value));
                }

                if (this.APPROVAL_TIME_FROM.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.APPROVAL_TIME >= this.APPROVAL_TIME_FROM.Value);
                }
                if (this.APPROVAL_DATE_FROM.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.APPROVAL_DATE >= this.APPROVAL_DATE_FROM.Value);
                }
                if (this.EXP_TIME_FROM.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_DATE_FROM.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_DATE.Value >= this.EXP_DATE_FROM.Value);
                }
                if (this.APPROVAL_TIME_TO.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.APPROVAL_TIME <= this.APPROVAL_TIME_TO.Value);
                }
                if (this.APPROVAL_DATE_TO.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.APPROVAL_DATE <= this.APPROVAL_DATE_TO.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }
                if (this.EXP_DATE_TO.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.EXP_DATE.Value <= this.EXP_DATE_TO.Value);
                }
                if (this.IS_EXPEND.HasValue && this.IS_EXPEND.Value)
                {
                    listHisExpMestMedicineExpression.Add(o => o.IS_EXPEND.HasValue && o.IS_EXPEND.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && this.IS_EXPORT.Value)
                {
                    listHisExpMestMedicineExpression.Add(o => o.IS_EXPORT.HasValue && o.IS_EXPORT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPEND.HasValue && !this.IS_EXPEND.Value)
                {
                    listHisExpMestMedicineExpression.Add(o => !o.IS_EXPEND.HasValue || o.IS_EXPEND.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && !this.IS_EXPORT.Value)
                {
                    listHisExpMestMedicineExpression.Add(o => !o.IS_EXPORT.HasValue || o.IS_EXPORT.Value != ManagerConstant.IS_TRUE);
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listHisExpMestMedicineExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && o.PATIENT_TYPE_ID.Value == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID.Value));
                }
                if (this.TDL_SERVICE_REQ_IDs != null)
                {
                    listHisExpMestMedicineExpression.Add(o => o.TDL_SERVICE_REQ_ID.HasValue && this.TDL_SERVICE_REQ_IDs.Contains(o.TDL_SERVICE_REQ_ID.Value));
                }

                search.listHisExpMestMedicineExpression.AddRange(listHisExpMestMedicineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestMedicineExpression.Clear();
                search.listHisExpMestMedicineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
