using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMedicine
{
    public class HisExpMestMedicineViewFilterQuery : HisExpMestMedicineViewFilter
    {
        public HisExpMestMedicineViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE, bool>>> listVHisExpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE, bool>>>();

        internal HisExpMestMedicineSO Query()
        {
            HisExpMestMedicineSO search = new HisExpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMedicineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMedicineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.HAS_MEDI_STOCK_PERIOD.HasValue)
                {
                    if (this.HAS_MEDI_STOCK_PERIOD.Value)
                    {
                        listVHisExpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID != null);
                    }
                    else
                    {
                        listVHisExpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == null);
                    }
                }


                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisExpMestMedicineExpression.Add(o => o.APP_CREATOR.Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.Contains(this.KEY_WORD) ||
                        o.CREATOR.Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.Contains(this.KEY_WORD) ||
                        o.TUTORIAL.Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGR_BHYT_CODE.Contains(this.KEY_WORD) ||
                        o.ACTIVE_INGR_BHYT_NAME.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_CODE.Contains(this.KEY_WORD) ||
                        o.MEDICINE_REGISTER_NUMBER.Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_NAME.Contains(this.KEY_WORD) ||
                        o.NATIONAL_NAME.Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.Contains(this.KEY_WORD) ||
                        o.REGISTER_NUMBER.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.Contains(this.KEY_WORD)
                        );
                }

                if (this.TDL_MEDICINE_TYPE_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.TDL_MEDICINE_TYPE_ID == o.TDL_MEDICINE_TYPE_ID);
                }
                if (this.EXP_MEST_TYPE_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.EXP_MEST_TYPE_ID == o.EXP_MEST_TYPE_ID);
                }
                if (this.AGGR_EXP_MEST_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_ID == o.AGGR_EXP_MEST_ID);
                }
                if (this.MEDI_STOCK_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.MEDI_STOCK_ID == o.MEDI_STOCK_ID);
                }
                if (this.MEDI_STOCK_PERIOD_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_ID.Value == o.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.EXP_MEST_STT_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.EXP_MEST_STT_ID == o.EXP_MEST_STT_ID);
                }
                if (this.REQ_ROOM_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.REQ_ROOM_ID.Value == o.REQ_ROOM_ID);
                }
                if (this.REQ_DEPARTMENT_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.REQ_DEPARTMENT_ID.Value == o.REQ_DEPARTMENT_ID);
                }
                if (this.BID_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.BID_ID.HasValue && this.BID_ID.Value == o.BID_ID.Value);
                }
                if (this.MEDICINE_TYPE_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.MEDICINE_TYPE_ID == o.MEDICINE_TYPE_ID);
                }
                if (this.SUPPLIER_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_ID.Value == o.SUPPLIER_ID.Value);
                }
                if (this.SERVICE_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.SERVICE_ID == o.SERVICE_ID);
                }
                if (this.MANUFACTURER_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MANUFACTURER_ID.HasValue && this.MANUFACTURER_ID.Value == o.MANUFACTURER_ID.Value);
                }
                if (this.MEMA_GROUP_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEMA_GROUP_ID.HasValue && this.MEMA_GROUP_ID.Value == o.MEMA_GROUP_ID.Value);
                }
                if (this.SERVICE_UNIT_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.SERVICE_UNIT_ID == o.SERVICE_UNIT_ID);
                }
                if (this.EXP_MEST_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.EXP_MEST_ID == o.EXP_MEST_ID);
                }
                if (this.MEDICINE_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.MEDICINE_ID == o.MEDICINE_ID);
                }
                if (this.TDL_MEDI_STOCK_ID != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.TDL_MEDI_STOCK_ID == o.TDL_MEDI_STOCK_ID);
                }
                if (this.TDL_MEDICINE_TYPE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.TDL_MEDICINE_TYPE_ID != null && this.TDL_MEDICINE_TYPE_IDs.Contains(o.TDL_MEDICINE_TYPE_ID.Value));
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.AGGR_EXP_MEST_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_IDs.Contains(o.AGGR_EXP_MEST_ID.Value));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID.Value));
                }
                if (this.EXP_MEST_STT_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.EXP_MEST_STT_IDs.Contains(o.EXP_MEST_STT_ID));
                }
                if (this.REQ_ROOM_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID));
                }
                if (this.REQ_DEPARTMENT_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID));
                }
                if (this.BID_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.SUPPLIER_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_IDs.Contains(o.SUPPLIER_ID.Value));
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.MANUFACTURER_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MANUFACTURER_ID.HasValue && this.MANUFACTURER_IDs.Contains(o.MANUFACTURER_ID.Value));
                }
                if (this.MEMA_GROUP_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEMA_GROUP_ID.HasValue && this.MEMA_GROUP_IDs.Contains(o.MEMA_GROUP_ID.Value));
                }
                if (this.SERVICE_UNIT_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => this.SERVICE_UNIT_IDs.Contains(o.SERVICE_UNIT_ID));
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEDICINE_ID != null && this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.TDL_MEDI_STOCK_ID != null && this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID.Value));
                }

                if (this.EXP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_DATE_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value >= this.EXP_DATE_FROM.Value);
                }
                if (this.EXPIRED_DATE_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value >= this.EXPIRED_DATE_FROM.Value);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.APPROVAL_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.APPROVAL_TIME >= this.APPROVAL_TIME_FROM.Value);
                }
                if (this.APPROVAL_DATE_FROM.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.APPROVAL_DATE >= this.APPROVAL_DATE_FROM.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }
                if (this.EXP_DATE_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value <= this.EXP_DATE_TO.Value);
                }
                if (this.EXPIRED_DATE_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value <= this.EXPIRED_DATE_TO.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }
                if (this.APPROVAL_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.APPROVAL_TIME <= this.APPROVAL_TIME_TO.Value);
                }
                if (this.APPROVAL_DATE_TO.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.APPROVAL_DATE <= this.APPROVAL_DATE_TO.Value);
                }
                if (this.IS_EXPEND.HasValue && this.IS_EXPEND.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IS_EXPEND.HasValue && o.IS_EXPEND.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && this.IS_EXPORT.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IS_EXPORT.HasValue && o.IS_EXPORT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.MEDICINE_GROUP_ID.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.MEDICINE_GROUP_ID.HasValue && o.MEDICINE_GROUP_ID.Value == this.MEDICINE_GROUP_ID.Value);
                }
                if (this.IS_FUNCTIONAL_FOOD.HasValue && this.IS_FUNCTIONAL_FOOD.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.IS_FUNCTIONAL_FOOD.HasValue && o.IS_FUNCTIONAL_FOOD.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPEND.HasValue && !this.IS_EXPEND.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => !o.IS_EXPEND.HasValue || o.IS_EXPEND.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && !this.IS_EXPORT.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => !o.IS_EXPORT.HasValue || o.IS_EXPORT.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_FUNCTIONAL_FOOD.HasValue && !this.IS_FUNCTIONAL_FOOD.Value)
                {
                    listVHisExpMestMedicineExpression.Add(o => !o.IS_FUNCTIONAL_FOOD.HasValue || o.IS_FUNCTIONAL_FOOD.Value != ManagerConstant.IS_TRUE);
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && o.PATIENT_TYPE_ID.Value == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID.Value));
                }
                if (this.TDL_SERVICE_REQ_IDs != null)
                {
                    listVHisExpMestMedicineExpression.Add(o => o.TDL_SERVICE_REQ_ID.HasValue && this.TDL_SERVICE_REQ_IDs.Contains(o.TDL_SERVICE_REQ_ID.Value));
                }

                search.listVHisExpMestMedicineExpression.AddRange(listVHisExpMestMedicineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMedicineExpression.Clear();
                search.listVHisExpMestMedicineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
