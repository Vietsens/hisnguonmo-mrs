using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttTable
{
    public class HisPtttTableFilterQuery : HisPtttTableFilter
    {
        public HisPtttTableFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PTTT_TABLE, bool>>> listHisPtttTableExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PTTT_TABLE, bool>>>();

        

        internal HisPtttTableSO Query()
        {
            HisPtttTableSO search = new HisPtttTableSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisPtttTableExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPtttTableExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPtttTableExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPtttTableExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPtttTableExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.PTTT_TABLE_CODE__EXACT))
                {
                    listHisPtttTableExpression.Add(o => o.PTTT_TABLE_CODE == this.PTTT_TABLE_CODE__EXACT);
                }
                
                search.listHisPtttTableExpression.AddRange(listHisPtttTableExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPtttTableExpression.Clear();
                search.listHisPtttTableExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
