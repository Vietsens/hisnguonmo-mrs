using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMachine
{
    public class HisServiceMachineFilterQuery : HisServiceMachineFilter
    {
        public HisServiceMachineFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MACHINE, bool>>> listHisServiceMachineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MACHINE, bool>>>();



        internal HisServiceMachineSO Query()
        {
            HisServiceMachineSO search = new HisServiceMachineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisServiceMachineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisServiceMachineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisServiceMachineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisServiceMachineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisServiceMachineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisServiceMachineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.SERVICE_ID != null)
                {
                    listHisServiceMachineExpression.Add(o => this.SERVICE_ID.Value == o.SERVICE_ID);
                }
                if (this.MACHINE_ID != null)
                {
                    listHisServiceMachineExpression.Add(o => this.MACHINE_ID.Value == o.MACHINE_ID);
                }
                if (this.SERVICE_IDs != null)
                {
                    listHisServiceMachineExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.MACHINE_IDs != null)
                {
                    listHisServiceMachineExpression.Add(o => this.MACHINE_IDs.Contains(o.MACHINE_ID));
                }

                search.listHisServiceMachineExpression.AddRange(listHisServiceMachineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServiceMachineExpression.Clear();
                search.listHisServiceMachineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
