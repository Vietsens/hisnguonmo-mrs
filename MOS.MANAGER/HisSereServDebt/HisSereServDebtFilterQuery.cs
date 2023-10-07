using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDebt
{
    public class HisSereServDebtFilterQuery : HisSereServDebtFilter
    {
        public HisSereServDebtFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEBT, bool>>> listHisSereServDebtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEBT, bool>>>();



        internal HisSereServDebtSO Query()
        {
            HisSereServDebtSO search = new HisSereServDebtSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSereServDebtExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSereServDebtExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSereServDebtExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.DEBT_ID.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.DEBT_ID == this.DEBT_ID.Value);
                }
                if (this.DEBT_IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.DEBT_IDs.Contains(o.DEBT_ID));
                }
                if (this.SERE_SERV_ID.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.IS_CANCEL.HasValue)
                {
                    if (this.IS_CANCEL.Value)
                    {
                        listHisSereServDebtExpression.Add(o => o.IS_CANCEL.HasValue && o.IS_CANCEL.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        listHisSereServDebtExpression.Add(o => !o.IS_CANCEL.HasValue || o.IS_CANCEL.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.TDL_TREATMENT_ID.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.TDL_TREATMENT_ID == this.TDL_TREATMENT_ID.Value);
                }
                if (this.TDL_TREATMENT_IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID));
                }
                if (this.TDL_SERVICE_ID.HasValue)
                {
                    listHisSereServDebtExpression.Add(o => o.TDL_SERVICE_ID == this.TDL_SERVICE_ID.Value);
                }
                if (this.TDL_SERVICE_IDs != null)
                {
                    listHisSereServDebtExpression.Add(o => this.TDL_SERVICE_IDs.Contains(o.TDL_SERVICE_ID));
                }

                search.listHisSereServDebtExpression.AddRange(listHisSereServDebtExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServDebtExpression.Clear();
                search.listHisSereServDebtExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
