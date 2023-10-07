using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMaterial
{
    public class HisExpMestMaterialViewFilterQuery : HisExpMestMaterialViewFilter
    {
        public HisExpMestMaterialViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL, bool>>> listVHisExpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL, bool>>>();

        internal HisExpMestMaterialSO Query()
        {
            HisExpMestMaterialSO search = new HisExpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMaterialExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMaterialExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.HAS_MEDI_STOCK_PERIOD.HasValue)
                {
                    if (this.HAS_MEDI_STOCK_PERIOD.Value)
                    {
                        listVHisExpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID != null);
                    }
                    else
                    {
                        listVHisExpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == null);
                    }
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisExpMestMaterialExpression.Add(o => o.APP_CREATOR.Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.Contains(this.KEY_WORD) ||
                        o.CREATOR.Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_CODE.Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_NAME.Contains(this.KEY_WORD) ||
                        o.NATIONAL_NAME.Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.Contains(this.KEY_WORD)
                        );
                }

                if (this.TDL_MATERIAL_TYPE_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.TDL_MATERIAL_TYPE_ID == o.TDL_MATERIAL_TYPE_ID);
                }
                if (this.EXP_MEST_TYPE_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.EXP_MEST_TYPE_ID == o.EXP_MEST_TYPE_ID);
                }
                if (this.AGGR_EXP_MEST_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_ID == o.AGGR_EXP_MEST_ID);
                }
                if (this.MEDI_STOCK_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.MEDI_STOCK_ID == o.MEDI_STOCK_ID);
                }
                if (this.MEDI_STOCK_PERIOD_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_ID.Value == o.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.EXP_MEST_STT_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.EXP_MEST_STT_ID == o.EXP_MEST_STT_ID);
                }
                if (this.REQ_ROOM_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.REQ_ROOM_ID.Value == o.REQ_ROOM_ID);
                }
                if (this.REQ_DEPARTMENT_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.REQ_DEPARTMENT_ID.Value == o.REQ_DEPARTMENT_ID);
                }
                if (this.BID_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.BID_ID.HasValue && this.BID_ID.Value == o.BID_ID.Value);
                }
                if (this.MATERIAL_TYPE_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.MATERIAL_TYPE_ID == o.MATERIAL_TYPE_ID);
                }
                if (this.SUPPLIER_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_ID.Value == o.SUPPLIER_ID.Value);
                }
                if (this.SERVICE_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.SERVICE_ID == o.SERVICE_ID);
                }
                if (this.MANUFACTURER_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MANUFACTURER_ID.HasValue && this.MANUFACTURER_ID.Value == o.MANUFACTURER_ID.Value);
                }
                if (this.MEMA_GROUP_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MEMA_GROUP_ID.HasValue && this.MEMA_GROUP_ID.Value == o.MEMA_GROUP_ID.Value);
                }
                if (this.SERVICE_UNIT_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.SERVICE_UNIT_ID == o.SERVICE_UNIT_ID);
                }
                if (this.EXP_MEST_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.EXP_MEST_ID == o.EXP_MEST_ID);
                }
                if (this.MATERIAL_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.MATERIAL_ID == o.MATERIAL_ID);
                }
                if (this.TDL_MEDI_STOCK_ID != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.TDL_MEDI_STOCK_ID == o.TDL_MEDI_STOCK_ID);
                }
                if (this.TDL_MATERIAL_TYPE_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.TDL_MATERIAL_TYPE_ID != null && this.TDL_MATERIAL_TYPE_IDs.Contains(o.TDL_MATERIAL_TYPE_ID.Value));
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.AGGR_EXP_MEST_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_IDs.Contains(o.AGGR_EXP_MEST_ID.Value));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID.Value));
                }
                if (this.EXP_MEST_STT_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.EXP_MEST_STT_IDs.Contains(o.EXP_MEST_STT_ID));
                }
                if (this.REQ_ROOM_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID));
                }
                if (this.REQ_DEPARTMENT_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID));
                }
                if (this.BID_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID));
                }
                if (this.SUPPLIER_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_IDs.Contains(o.SUPPLIER_ID.Value));
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.MANUFACTURER_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MANUFACTURER_ID.HasValue && this.MANUFACTURER_IDs.Contains(o.MANUFACTURER_ID.Value));
                }
                if (this.MEMA_GROUP_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MEMA_GROUP_ID.HasValue && this.MEMA_GROUP_IDs.Contains(o.MEMA_GROUP_ID.Value));
                }
                if (this.SERVICE_UNIT_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => this.SERVICE_UNIT_IDs.Contains(o.SERVICE_UNIT_ID));
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.MATERIAL_ID != null && this.MATERIAL_IDs.Contains(o.MATERIAL_ID.Value));
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.TDL_MEDI_STOCK_ID != null && this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID.Value));
                }

                if (this.EXP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_DATE_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value >= this.EXP_DATE_FROM.Value);
                }
                if (this.EXPIRED_DATE_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value >= this.EXPIRED_DATE_FROM.Value);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.APPROVAL_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.APPROVAL_TIME >= this.APPROVAL_TIME_FROM.Value);
                }
                if (this.APPROVAL_DATE_FROM.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.APPROVAL_DATE >= this.APPROVAL_DATE_FROM.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }
                if (this.EXP_DATE_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value <= this.EXP_DATE_TO.Value);
                }
                if (this.EXPIRED_DATE_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value <= this.EXPIRED_DATE_TO.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }
                if (this.APPROVAL_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.APPROVAL_TIME <= this.APPROVAL_TIME_TO.Value);
                }
                if (this.APPROVAL_DATE_TO.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.APPROVAL_DATE <= this.APPROVAL_DATE_TO.Value);
                }
                if (this.IS_EXPEND.HasValue && this.IS_EXPEND.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IS_EXPEND.HasValue && o.IS_EXPEND.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && this.IS_EXPORT.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IS_EXPORT.HasValue && o.IS_EXPORT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CHEMICAL_SUBSTANCE.HasValue && this.IS_CHEMICAL_SUBSTANCE.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.IS_CHEMICAL_SUBSTANCE.HasValue && o.IS_CHEMICAL_SUBSTANCE.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPEND.HasValue && !this.IS_EXPEND.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => !o.IS_EXPEND.HasValue || o.IS_EXPEND.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && !this.IS_EXPORT.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => !o.IS_EXPORT.HasValue || o.IS_EXPORT.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_CHEMICAL_SUBSTANCE.HasValue && !this.IS_CHEMICAL_SUBSTANCE.Value)
                {
                    listVHisExpMestMaterialExpression.Add(o => !o.IS_CHEMICAL_SUBSTANCE.HasValue || o.IS_CHEMICAL_SUBSTANCE.Value != ManagerConstant.IS_TRUE);
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && o.PATIENT_TYPE_ID.Value == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    listVHisExpMestMaterialExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID.Value));
                }

                search.listVHisExpMestMaterialExpression.AddRange(listVHisExpMestMaterialExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMaterialExpression.Clear();
                search.listVHisExpMestMaterialExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
