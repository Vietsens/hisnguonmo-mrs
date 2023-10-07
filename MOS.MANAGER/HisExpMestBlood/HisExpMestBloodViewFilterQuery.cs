using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBlood
{
    public class HisExpMestBloodViewFilterQuery : HisExpMestBloodViewFilter
    {
        public HisExpMestBloodViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLOOD, bool>>> listVHisExpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLOOD, bool>>>();

        internal HisExpMestBloodSO Query()
        {
            HisExpMestBloodSO search = new HisExpMestBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.HAS_MEDI_STOCK_PERIOD.HasValue)
                {
                    if (this.HAS_MEDI_STOCK_PERIOD.Value)
                    {
                        listVHisExpMestBloodExpression.Add(o => o.MEDI_STOCK_PERIOD_ID != null);
                    }
                    else
                    {
                        listVHisExpMestBloodExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == null);
                    }
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisExpMestBloodExpression.Add(o => o.APP_CREATOR.Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.Contains(this.KEY_WORD) ||
                        o.CREATOR.Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_CODE.Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_NAME.Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.Contains(this.KEY_WORD)
                        );
                }

                if (this.EXP_MEST_TYPE_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_TYPE_ID == o.EXP_MEST_TYPE_ID);
                }
                if (this.AGGR_EXP_MEST_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_ID == o.AGGR_EXP_MEST_ID);
                }
                if (this.MEDI_STOCK_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.MEDI_STOCK_ID == o.MEDI_STOCK_ID);
                }
                if (this.MEDI_STOCK_PERIOD_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_ID.Value == o.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.EXP_MEST_STT_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_STT_ID == o.EXP_MEST_STT_ID);
                }
                if (this.REQ_ROOM_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.REQ_ROOM_ID.Value == o.REQ_ROOM_ID);
                }
                if (this.REQ_DEPARTMENT_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.REQ_DEPARTMENT_ID.Value == o.REQ_DEPARTMENT_ID);
                }
                if (this.BID_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.BID_ID.HasValue && this.BID_ID.Value == o.BID_ID.Value);
                }
                if (this.BLOOD_TYPE_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.BLOOD_TYPE_ID == o.BLOOD_TYPE_ID);
                }
                if (this.SUPPLIER_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_ID.Value == o.SUPPLIER_ID.Value);
                }
                if (this.SERVICE_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.SERVICE_ID == o.SERVICE_ID);
                }
                if (this.SERVICE_UNIT_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.SERVICE_UNIT_ID == o.SERVICE_UNIT_ID);
                }
                if (this.EXP_MEST_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_ID == o.EXP_MEST_ID);
                }
                if (this.BLOOD_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.BLOOD_ID == o.BLOOD_ID);
                }
                if (this.TDL_MEDI_STOCK_ID != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.TDL_MEDI_STOCK_ID == o.TDL_MEDI_STOCK_ID);
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.AGGR_EXP_MEST_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.AGGR_EXP_MEST_ID.HasValue && this.AGGR_EXP_MEST_IDs.Contains(o.AGGR_EXP_MEST_ID.Value));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }
                if (this.MEDI_STOCK_PERIOD_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.MEDI_STOCK_PERIOD_ID.HasValue && this.MEDI_STOCK_PERIOD_IDs.Contains(o.MEDI_STOCK_PERIOD_ID.Value));
                }
                if (this.EXP_MEST_STT_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_STT_IDs.Contains(o.EXP_MEST_STT_ID));
                }
                if (this.REQ_ROOM_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID));
                }
                if (this.REQ_DEPARTMENT_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID));
                }
                if (this.BID_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }
                if (this.SUPPLIER_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => o.SUPPLIER_ID.HasValue && this.SUPPLIER_IDs.Contains(o.SUPPLIER_ID.Value));
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.SERVICE_UNIT_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.SERVICE_UNIT_IDs.Contains(o.SERVICE_UNIT_ID));
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID));
                }
                if (this.BLOOD_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.BLOOD_IDs.Contains(o.BLOOD_ID));
                }
                if (this.TDL_MEDI_STOCK_IDs != null)
                {
                    listVHisExpMestBloodExpression.Add(o => this.TDL_MEDI_STOCK_IDs.Contains(o.TDL_MEDI_STOCK_ID));
                }

                if (this.EXP_TIME_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_DATE_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value >= this.EXP_DATE_FROM.Value);
                }
                if (this.EXPIRED_DATE_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value >= this.EXPIRED_DATE_FROM.Value);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.APPROVAL_TIME_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.APPROVAL_TIME >= this.APPROVAL_TIME_FROM.Value);
                }
                if (this.APPROVAL_DATE_FROM.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.APPROVAL_DATE >= this.APPROVAL_DATE_FROM.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXP_TIME.HasValue && o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }
                if (this.EXP_DATE_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXP_DATE.HasValue && o.EXP_DATE.Value <= this.EXP_DATE_TO.Value);
                }
                if (this.EXPIRED_DATE_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.EXPIRED_DATE.HasValue && o.EXPIRED_DATE.Value <= this.EXPIRED_DATE_TO.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }
                if (this.APPROVAL_TIME_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.APPROVAL_TIME <= this.APPROVAL_TIME_TO.Value);
                }
                if (this.APPROVAL_DATE_TO.HasValue)
                {
                    listVHisExpMestBloodExpression.Add(o => o.APPROVAL_DATE <= this.APPROVAL_DATE_TO.Value);
                }
                if (this.IS_EXPORT.HasValue && this.IS_EXPORT.Value)
                {
                    listVHisExpMestBloodExpression.Add(o => o.IS_EXPORT.HasValue && o.IS_EXPORT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT.HasValue && !this.IS_EXPORT.Value)
                {
                    listVHisExpMestBloodExpression.Add(o => !o.IS_EXPORT.HasValue || o.IS_EXPORT.Value != ManagerConstant.IS_TRUE);
                }

                search.listVHisExpMestBloodExpression.AddRange(listVHisExpMestBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestBloodExpression.Clear();
                search.listVHisExpMestBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
