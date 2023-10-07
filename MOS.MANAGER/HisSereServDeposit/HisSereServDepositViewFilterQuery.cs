using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDeposit
{
    public class HisSereServDepositViewFilterQuery : HisSereServDepositViewFilter
    {
        public HisSereServDepositViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEPOSIT, bool>>> listVHisSereServDepositExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEPOSIT, bool>>>();

        

        internal HisSereServDepositSO Query()
        {
            HisSereServDepositSO search = new HisSereServDepositSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSereServDepositExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSereServDepositExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSereServDepositExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.DEPOSIT_ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.DEPOSIT_ID == this.DEPOSIT_ID.Value);
                }
                if (this.DEPOSIT_IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.DEPOSIT_IDs.Contains(o.DEPOSIT_ID));
                }
                if (this.SERE_SERV_ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.IS_CANCEL.HasValue)
                {
                    if (this.IS_CANCEL.Value)
                    {
                        listVHisSereServDepositExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listVHisSereServDepositExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.TDL_TREATMENT_ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.TDL_TREATMENT_ID == this.TDL_TREATMENT_ID.Value);
                }
                if (this.TDL_TREATMENT_IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID));
                }
                if (this.TDL_SERVICE_ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.TDL_SERVICE_ID == this.TDL_SERVICE_ID.Value);
                }
                if (this.TDL_SERVICE_IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.TDL_SERVICE_IDs.Contains(o.TDL_SERVICE_ID));
                }
                if (this.SERVICE_REQ_STT_ID.HasValue)
                {
                    listVHisSereServDepositExpression.Add(o => o.SERVICE_REQ_STT_ID.Value == this.SERVICE_REQ_STT_ID.Value);
                }
                if (this.SERVICE_REQ_STT_IDs != null)
                {
                    listVHisSereServDepositExpression.Add(o => this.SERVICE_REQ_STT_IDs.Contains(o.SERVICE_REQ_STT_ID.Value));
                }

                search.listVHisSereServDepositExpression.AddRange(listVHisSereServDepositExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSereServDepositExpression.Clear();
                search.listVHisSereServDepositExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
