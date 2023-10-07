using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServBill
{
    public class HisSereServBillFilterQuery : HisSereServBillFilter
    {
        public HisSereServBillFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_BILL, bool>>> listHisSereServBillExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_BILL, bool>>>();

        

        internal HisSereServBillSO Query()
        {
            HisSereServBillSO search = new HisSereServBillSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisSereServBillExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSereServBillExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSereServBillExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSereServBillExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSereServBillExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.SERE_SERV_ID.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listHisSereServBillExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.BILL_ID.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.BILL_ID == this.BILL_ID.Value);
                }
                if (this.BILL_IDs != null)
                {
                    listHisSereServBillExpression.Add(o => this.BILL_IDs.Contains(o.BILL_ID));
                }
                if (this.IS_NOT_CANCEL.HasValue && this.IS_NOT_CANCEL.Value)
                {
                    listHisSereServBillExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_NOT_CANCEL.HasValue && !this.IS_NOT_CANCEL.Value)
                {
                    listHisSereServBillExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL.Value == ManagerConstant.IS_TRUE);
                }
                if (this.TDL_TREATMENT_ID.HasValue)
                {
                    listHisSereServBillExpression.Add(o => o.TDL_TREATMENT_ID == this.TDL_TREATMENT_ID.Value);
                }
                if (this.TDL_TREATMENT_IDs != null)
                {
                    listHisSereServBillExpression.Add(o => this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID));
                }

                search.listHisSereServBillExpression.AddRange(listHisSereServBillExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServBillExpression.Clear();
                search.listHisSereServBillExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
