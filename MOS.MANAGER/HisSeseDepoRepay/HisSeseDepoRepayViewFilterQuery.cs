using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSeseDepoRepay
{
    public class HisSeseDepoRepayViewFilterQuery : HisSeseDepoRepayViewFilter
    {
        public HisSeseDepoRepayViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SESE_DEPO_REPAY, bool>>> listVHisSeseDepoRepayExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SESE_DEPO_REPAY, bool>>>();

        

        internal HisSeseDepoRepaySO Query()
        {
            HisSeseDepoRepaySO search = new HisSeseDepoRepaySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.REPAY_ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.REPAY_ID == this.REPAY_ID.Value);
                }
                if (this.REPAY_IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.REPAY_IDs.Contains(o.REPAY_ID));
                }
                if (this.SERE_SERV_DEPOSIT_ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.SERE_SERV_DEPOSIT_ID == this.SERE_SERV_DEPOSIT_ID.Value);
                }
                if (this.SERE_SERV_DEPOSIT_IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.SERE_SERV_DEPOSIT_IDs.Contains(o.SERE_SERV_DEPOSIT_ID));
                }
                if (this.IS_CANCEL.HasValue)
                {
                    if (this.IS_CANCEL.Value)
                    {
                        listVHisSeseDepoRepayExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listVHisSeseDepoRepayExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.TDL_TREATMENT_ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.TDL_TREATMENT_ID == this.TDL_TREATMENT_ID.Value);
                }
                if (this.TDL_TREATMENT_IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID));
                }
                if (this.TDL_SERVICE_ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.TDL_SERVICE_ID == this.TDL_SERVICE_ID.Value);
                }
                if (this.TDL_SERVICE_IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.TDL_SERVICE_IDs.Contains(o.TDL_SERVICE_ID));
                }
                if (this.SERE_SERV_ID.HasValue)
                {
                    listVHisSeseDepoRepayExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listVHisSeseDepoRepayExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }


                search.listVHisSeseDepoRepayExpression.AddRange(listVHisSeseDepoRepayExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSeseDepoRepayExpression.Clear();
                search.listVHisSeseDepoRepayExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
