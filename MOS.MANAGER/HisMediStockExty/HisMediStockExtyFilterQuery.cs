using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockExty
{
    public class HisMediStockExtyFilterQuery : HisMediStockExtyFilter
    {
        public HisMediStockExtyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_EXTY, bool>>> listHisMediStockExtyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK_EXTY, bool>>>();

        

        internal HisMediStockExtySO Query()
        {
            HisMediStockExtySO search = new HisMediStockExtySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisMediStockExtyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMediStockExtyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMediStockExtyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMediStockExtyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisMediStockExtyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listHisMediStockExtyExpression.Add(o => this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID));
                }
                if (this.EXP_MEST_TYPE_ID.HasValue)
                {
                    listHisMediStockExtyExpression.Add(o => o.EXP_MEST_TYPE_ID == this.EXP_MEST_TYPE_ID.Value);
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listHisMediStockExtyExpression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }

                search.listHisMediStockExtyExpression.AddRange(listHisMediStockExtyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMediStockExtyExpression.Clear();
                search.listHisMediStockExtyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
