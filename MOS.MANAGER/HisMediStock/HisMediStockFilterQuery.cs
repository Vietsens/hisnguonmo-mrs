using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
    public class HisMediStockFilterQuery : HisMediStockFilter
    {
        public HisMediStockFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK, bool>>> listHisMediStockExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_STOCK, bool>>>();

        

        internal HisMediStockSO Query()
        {
            HisMediStockSO search = new HisMediStockSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMediStockExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMediStockExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMediStockExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMediStockExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMediStockExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IS_ALLOW_IMP_SUPPLIER.HasValue)
                {
                    if (this.IS_ALLOW_IMP_SUPPLIER.Value)
                    {
                        search.listHisMediStockExpression.Add(o => o.IS_ALLOW_IMP_SUPPLIER == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        search.listHisMediStockExpression.Add(o => !o.IS_ALLOW_IMP_SUPPLIER.HasValue || o.IS_ALLOW_IMP_SUPPLIER.Value != ManagerConstant.IS_TRUE);
                    }
                }
                
                #endregion

                if (this.IsTree.HasValue && this.IsTree.Value)
                {
                    listHisMediStockExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
                }
                if (this.ROOM_ID.HasValue)
                {
                    listHisMediStockExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.PARENT_ID.HasValue)
                {
                    listHisMediStockExpression.Add(o => o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.PARENT_ID.Value);
                }
                if (this.IS_BUSINESS.HasValue && this.IS_BUSINESS.Value)
                {
                    listHisMediStockExpression.Add(o => o.IS_BUSINESS.HasValue && o.IS_BUSINESS.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_BUSINESS.HasValue && !this.IS_BUSINESS.Value)
                {
                    listHisMediStockExpression.Add(o => !o.IS_BUSINESS.HasValue || o.IS_BUSINESS.Value != ManagerConstant.IS_TRUE);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisMediStockExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.ROOM_IDs != null)
                {
                    listHisMediStockExpression.Add(o => this.ROOM_IDs.Contains(o.ROOM_ID));
                }
                
                search.listHisMediStockExpression.AddRange(listHisMediStockExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMediStockExpression.Clear();
                search.listHisMediStockExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
