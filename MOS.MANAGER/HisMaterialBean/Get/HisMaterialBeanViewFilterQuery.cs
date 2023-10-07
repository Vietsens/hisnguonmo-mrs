using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    public class HisMaterialBeanViewFilterQuery : HisMaterialBeanViewFilter
    {
        public HisMaterialBeanViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN, bool>>> listVHisMaterialBeanExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN, bool>>>();

        

        internal HisMaterialBeanSO Query()
        {
            HisMaterialBeanSO search = new HisMaterialBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMaterialBeanExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMaterialBeanExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMaterialBeanExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMaterialBeanExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID);
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID);
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID);
                }
                if (this.MATERIAL_TYPE_IS_ACTIVE.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MATERIAL_TYPE_IS_ACTIVE.HasValue && o.MATERIAL_TYPE_IS_ACTIVE.Value == this.MATERIAL_TYPE_IS_ACTIVE.Value);
                }
                if (this.MATERIAL_IS_ACTIVE.HasValue)
                {
                    listVHisMaterialBeanExpression.Add(o => o.MATERIAL_IS_ACTIVE.HasValue && o.MATERIAL_IS_ACTIVE.Value == this.MATERIAL_IS_ACTIVE.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisMaterialBeanExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID));
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisMaterialBeanExpression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                if (this.SOURCE_IDs != null)
                {
                    listVHisMaterialBeanExpression.Add(o => o.SOURCE_ID != null && this.SOURCE_IDs.Contains(o.SOURCE_ID.Value));
                }
                if (this.IN_STOCK.HasValue)
                {
                    switch (this.IN_STOCK.Value)
                    {
                        case InStockEnum.YES:
                            listVHisMaterialBeanExpression.Add(o => o.MEDI_STOCK_ID != null);
                            break;
                        case InStockEnum.NO:
                            listVHisMaterialBeanExpression.Add(o => o.MEDI_STOCK_ID == null);
                            break;
                        default:
                            break;
                    }
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    search.listVHisMaterialBeanExpression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMaterialBeanExpression.Add(o =>
                        o.MATERIAL_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.IS_GOODS_RESTRICT.HasValue && this.IS_GOODS_RESTRICT.Value)
                {
                    listVHisMaterialBeanExpression.Add(o => o.IS_GOODS_RESTRICT.HasValue && o.IS_GOODS_RESTRICT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_GOODS_RESTRICT.HasValue && !this.IS_GOODS_RESTRICT.Value)
                {
                    listVHisMaterialBeanExpression.Add(o => !o.IS_GOODS_RESTRICT.HasValue || o.IS_GOODS_RESTRICT.Value != ManagerConstant.IS_TRUE);
                }

                search.listVHisMaterialBeanExpression.AddRange(listVHisMaterialBeanExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMaterialBeanExpression.Clear();
                search.listVHisMaterialBeanExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
