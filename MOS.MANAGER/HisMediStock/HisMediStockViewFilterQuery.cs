using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
	public class HisMediStockViewFilterQuery : HisMediStockViewFilter
	{
		public HisMediStockViewFilterQuery()
			: base()
		{

		}
		
		internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK, bool>>> listVHisMediStockExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_STOCK, bool>>>();

		

		internal HisMediStockSO Query()
		{
			HisMediStockSO search = new HisMediStockSO();
			try
			{
				#region Abstract Base
				if (this.ID.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.ID == this.ID.Value);
				}
                if (this.IDs != null)
                {
                    search.listVHisMediStockExpression.Add(o => this.IDs.Contains(o.ID));
                }
				if (this.IS_ACTIVE.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
				}
				if (this.CREATE_TIME_FROM.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
				}
				if (this.CREATE_TIME_TO.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
				}
				if (this.MODIFY_TIME_FROM.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
				}
				if (this.MODIFY_TIME_TO.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
				}
				if (!String.IsNullOrEmpty(this.CREATOR))
				{
					search.listVHisMediStockExpression.Add(o => o.CREATOR == this.CREATOR);
				}
				if (!String.IsNullOrEmpty(this.MODIFIER))
				{
					search.listVHisMediStockExpression.Add(o => o.MODIFIER == this.MODIFIER);
				}
				if (!String.IsNullOrEmpty(this.GROUP_CODE))
				{
					search.listVHisMediStockExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
				}
				#endregion

                if (!String.IsNullOrEmpty(this.MEDI_STOCK_CODE))
                {
                    this.MEDI_STOCK_CODE = this.MEDI_STOCK_CODE.Trim().ToLower();
                    search.listVHisMediStockExpression.Add(o => o.MEDI_STOCK_CODE.ToLower().Contains(this.MEDI_STOCK_CODE));
                }
                if (this.IS_ALLOW_IMP_SUPPLIER.HasValue)
                {
                    if (this.IS_ALLOW_IMP_SUPPLIER.Value)
                    {
                        search.listVHisMediStockExpression.Add(o => o.IS_ALLOW_IMP_SUPPLIER == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        search.listVHisMediStockExpression.Add(o => !o.IS_ALLOW_IMP_SUPPLIER.HasValue || o.IS_ALLOW_IMP_SUPPLIER.Value != ManagerConstant.IS_TRUE);
                    }
                }
				if (this.IsTree.HasValue && this.IsTree.Value)
				{
					listVHisMediStockExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
				}
				if (this.DEPARTMENT_ID.HasValue)
				{
					search.listVHisMediStockExpression.Add(o => o.DEPARTMENT_ID == this.DEPARTMENT_ID.Value);
				}
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMediStockExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PARENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PARENT_NAME.ToLower().Contains(this.KEY_WORD)                        
                        );
                }
                if (this.IS_BUSINESS.HasValue && this.IS_BUSINESS.Value)
                {
                    listVHisMediStockExpression.Add(o => o.IS_BUSINESS.HasValue && o.IS_BUSINESS.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_BUSINESS.HasValue && !this.IS_BUSINESS.Value)
                {
                    listVHisMediStockExpression.Add(o => !o.IS_BUSINESS.HasValue || o.IS_BUSINESS.Value != ManagerConstant.IS_TRUE);
                }
				
				search.listVHisMediStockExpression.AddRange(listVHisMediStockExpression);
				search.OrderField = ORDER_FIELD;
				search.OrderDirection = ORDER_DIRECTION;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				search.listVHisMediStockExpression.Clear();
				search.listVHisMediStockExpression.Add(o => o.ID == NEGATIVE_ID);
			}
			return search;
		}
	}
}
