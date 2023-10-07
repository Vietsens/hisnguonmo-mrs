using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMest
{
    public class HisExpMestViewFilterQuery : HisExpMestViewFilter
    {
        public HisExpMestViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST, bool>>> listVHisExpMestExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST, bool>>>();

        internal HisExpMestSO Query()
        {
            HisExpMestSO search = new HisExpMestSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisExpMestExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisExpMestExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisExpMestExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisExpMestExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.BILL_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.BILL_ID.HasValue && this.BILL_ID.Value == o.BILL_ID.Value);
                }
                if (this.PRESCRIPTION_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.PRESCRIPTION_ID.HasValue && this.PRESCRIPTION_ID.Value == o.PRESCRIPTION_ID.Value);
                }
                if (this.MANU_IMP_MEST_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.MANU_IMP_MEST_ID.HasValue && this.MANU_IMP_MEST_ID.Value == o.MANU_IMP_MEST_ID.Value);
                }
                if (this.SUPPLIER_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_ID.Value == o.SUPPLIER_ID.Value);
                }
                if (this.AGGR_USE_TIME != null)
                {
                    listVHisExpMestExpression.Add(o => o.AGGR_USE_TIME.HasValue && this.AGGR_USE_TIME.Value == o.AGGR_USE_TIME.Value);
                }
                if (this.AGGR_EXP_MEST_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_ID.Value == o.AGGR_EXP_MEST_ID.Value);
                }
                if (this.IMP_MEDI_STOCK_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.IMP_MEDI_STOCK_ID.HasValue && this.IMP_MEDI_STOCK_ID.Value == o.IMP_MEDI_STOCK_ID.Value);
                }
                if (this.IMP_OR_EXP_MEDI_STOCK_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.MEDI_STOCK_ID == this.IMP_OR_EXP_MEDI_STOCK_ID.Value || (o.IMP_MEDI_STOCK_ID.HasValue && this.IMP_OR_EXP_MEDI_STOCK_ID.Value == o.IMP_MEDI_STOCK_ID.Value));
                }
                if (this.TDL_TREATMENT_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TDL_TREATMENT_ID.Value == o.TDL_TREATMENT_ID.Value);
                }
                if (this.SERVICE_REQ_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_ID.Value == o.SERVICE_REQ_ID.Value);
                }
                if (this.EXP_MEST_REASON_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.EXP_MEST_REASON_ID.HasValue && this.EXP_MEST_REASON_ID.Value == o.EXP_MEST_REASON_ID.Value);
                }
                if (this.REQ_DEPARTMENT_ID != null)
                {
                    listVHisExpMestExpression.Add(o => this.REQ_DEPARTMENT_ID.Value == o.REQ_DEPARTMENT_ID);
                }
                if (this.MEDI_STOCK_ID != null)
                {
                    listVHisExpMestExpression.Add(o => this.MEDI_STOCK_ID.Value == o.MEDI_STOCK_ID);
                }
                if (this.EXP_MEST_STT_ID != null)
                {
                    listVHisExpMestExpression.Add(o => this.EXP_MEST_STT_ID.Value == o.EXP_MEST_STT_ID);
                }
                if (this.EXP_MEST_TYPE_ID != null)
                {
                    listVHisExpMestExpression.Add(o => this.EXP_MEST_TYPE_ID.Value == o.EXP_MEST_TYPE_ID);
                }
                if (this.SALE_PATIENT_TYPE_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.SALE_PATIENT_TYPE_ID.HasValue && this.SALE_PATIENT_TYPE_ID.Value == o.SALE_PATIENT_TYPE_ID.Value);
                }
                if (this.TDL_PATIENT_ID != null)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_PATIENT_ID.HasValue && this.TDL_PATIENT_ID.Value == o.TDL_PATIENT_ID.Value);
                }
                if (this.BILL_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.BILL_ID.HasValue && this.BILL_IDs.Contains(o.BILL_ID.Value));
                }
                if (this.PRESCRIPTION_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.PRESCRIPTION_ID.HasValue && this.PRESCRIPTION_IDs.Contains(o.PRESCRIPTION_ID.Value));
                }
                if (this.MANU_IMP_MEST_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.MANU_IMP_MEST_ID.HasValue && this.MANU_IMP_MEST_IDs.Contains(o.MANU_IMP_MEST_ID.Value));
                }
                if (this.SUPPLIER_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_IDs.Contains(o.SUPPLIER_ID.Value));
                }
                if (this.AGGR_USE_TIMEs != null)
                {
                    listVHisExpMestExpression.Add(o => o.AGGR_USE_TIME.HasValue && this.AGGR_USE_TIMEs.Contains(o.AGGR_USE_TIME.Value));
                }
                if (this.AGGR_EXP_MEST_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_IDs.Contains(o.AGGR_EXP_MEST_ID.Value));
                }
                if (this.IMP_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.IMP_MEDI_STOCK_ID.HasValue && this.IMP_MEDI_STOCK_IDs.Contains(o.IMP_MEDI_STOCK_ID.Value));
                }
                if (this.TDL_TREATMENT_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID.Value));
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID.Value));
                }
                if (this.EXP_MEST_REASON_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.EXP_MEST_REASON_ID.HasValue && this.EXP_MEST_REASON_IDs.Contains(o.EXP_MEST_REASON_ID.Value));
                }
                if (this.REQ_DEPARTMENT_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }
                if (this.EXP_MEST_STT_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.EXP_MEST_STT_IDs.Contains(o.EXP_MEST_STT_ID));
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.SALE_PATIENT_TYPE_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.SALE_PATIENT_TYPE_ID.HasValue && this.SALE_PATIENT_TYPE_IDs.Contains(o.SALE_PATIENT_TYPE_ID.Value));
                }
                if (this.TDL_PATIENT_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_PATIENT_ID.HasValue && this.TDL_PATIENT_IDs.Contains(o.TDL_PATIENT_ID.Value));
                }

                if (this.CREATE_DATE_FROM.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.CREATE_DATE >= this.CREATE_DATE_FROM.Value);
                }
                if (this.CREATE_DATE_TO.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.CREATE_DATE <= this.CREATE_DATE_TO.Value);
                }
                if (this.FINISH_DATE_FROM.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.FINISH_DATE.Value >= this.FINISH_DATE_FROM.Value);
                }
                if (this.FINISH_DATE_TO.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.FINISH_DATE.Value <= this.FINISH_DATE_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.FINISH_TIME.Value >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.FINISH_TIME.Value <= this.FINISH_TIME_TO.Value);
                }
                if (this.TDL_INTRUCTION_DATE_FROM.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_INTRUCTION_DATE.HasValue && o.TDL_INTRUCTION_DATE.Value >= this.TDL_INTRUCTION_DATE_FROM.Value);
                }
                if (this.TDL_INTRUCTION_DATE_TO.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.TDL_INTRUCTION_DATE.HasValue && o.TDL_INTRUCTION_DATE.Value <= this.TDL_INTRUCTION_DATE_TO.Value);
                }
                if (this.REQ_ROOM_ID.HasValue)
                {
                    listVHisExpMestExpression.Add(o => o.REQ_ROOM_ID == this.REQ_ROOM_ID.Value);
                }
                if (this.REQ_ROOM_IDs != null)
                {
                    listVHisExpMestExpression.Add(o => this.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID));
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisExpMestExpression.Add(o => o.EXP_MEST_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_LOGINNAME.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_USERNAME.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.REQ_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.EXP_MEST_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (!String.IsNullOrEmpty(this.EXP_MEST_CODE__EXACT))
                {
                    listVHisExpMestExpression.Add(o => o.EXP_MEST_CODE == this.EXP_MEST_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.TDL_PATIENT_CODE__EXACT))
                {
                    listVHisExpMestExpression.Add(o => o.TDL_PATIENT_CODE == this.TDL_PATIENT_CODE__EXACT);
                }
                if (this.HAS_AGGR.HasValue && this.HAS_AGGR.Value)
                {
                    search.listVHisExpMestExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue);
                }
                if (this.HAS_AGGR.HasValue && !this.HAS_AGGR.Value)
                {
                    search.listVHisExpMestExpression.Add(o => !o.AGGR_EXP_MEST_ID.HasValue);
                }
                if (this.IS_NOT_TAKEN.HasValue && this.IS_NOT_TAKEN.Value)
                {
                    search.listVHisExpMestExpression.Add(o => o.IS_NOT_TAKEN.HasValue && o.IS_NOT_TAKEN.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_NOT_TAKEN.HasValue && !this.IS_NOT_TAKEN.Value)
                {
                    search.listVHisExpMestExpression.Add(o => !o.IS_NOT_TAKEN.HasValue || o.IS_NOT_TAKEN.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_CABINET.HasValue && this.IS_CABINET.Value)
                {
                    search.listVHisExpMestExpression.Add(o => o.IS_CABINET.HasValue && o.IS_CABINET.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_CABINET.HasValue && !this.IS_CABINET.Value)
                {
                    search.listVHisExpMestExpression.Add(o => !o.IS_CABINET.HasValue || o.IS_CABINET.Value != ManagerConstant.IS_TRUE);
                }
                if (this.HAS_XBTT_EXP_MEST_ID.HasValue)
                {
                    if (this.HAS_XBTT_EXP_MEST_ID.Value)
                    {
                        listVHisExpMestExpression.Add(o => o.XBTT_EXP_MEST_ID.HasValue);
                    }
                    else
                    {
                        listVHisExpMestExpression.Add(o => !o.XBTT_EXP_MEST_ID.HasValue);
                    }
                }

                search.listVHisExpMestExpression.AddRange(listVHisExpMestExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestExpression.Clear();
                search.listVHisExpMestExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
