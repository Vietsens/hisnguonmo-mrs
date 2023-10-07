using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public class HisMedicineBeanViewFilterQuery : HisMedicineBeanViewFilter
    {
        public HisMedicineBeanViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN, bool>>> listVHisMedicineBeanExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN, bool>>>();

        

        internal HisMedicineBeanSO Query()
        {
            HisMedicineBeanSO search = new HisMedicineBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicineBeanExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicineBeanExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicineBeanExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicineBeanExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID);
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID);
                }
                if (this.MEDICINE_TYPE_IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MEDICINE_TYPE_IS_ACTIVE.HasValue && o.MEDICINE_TYPE_IS_ACTIVE.Value == this.MEDICINE_TYPE_IS_ACTIVE.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisMedicineBeanExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisMedicineBeanExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.SOURCE_IDs != null)
                {
                    listVHisMedicineBeanExpression.Add(o => o.SOURCE_ID != null && this.SOURCE_IDs.Contains(o.SOURCE_ID.Value));
                }
                if (this.IN_STOCK.HasValue)
                {
                    switch (this.IN_STOCK.Value)
                    {
                        case InStockEnum.YES:
                            listVHisMedicineBeanExpression.Add(o => o.MEDI_STOCK_ID != null);
                            break;
                        case InStockEnum.NO:
                            listVHisMedicineBeanExpression.Add(o => o.MEDI_STOCK_ID == null);
                            break;
                        default:
                            break;
                    }
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    search.listVHisMedicineBeanExpression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicineBeanExpression.Add(o =>
                        o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.MEDICINE_IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBeanExpression.Add(o => o.MEDICINE_IS_ACTIVE.HasValue && o.MEDICINE_IS_ACTIVE.Value == this.MEDICINE_IS_ACTIVE.Value);
                }
                if (this.IS_GOODS_RESTRICT.HasValue && this.IS_GOODS_RESTRICT.Value)
                {
                    listVHisMedicineBeanExpression.Add(o => o.IS_GOODS_RESTRICT.HasValue && o.IS_GOODS_RESTRICT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_GOODS_RESTRICT.HasValue && !this.IS_GOODS_RESTRICT.Value)
                {
                    listVHisMedicineBeanExpression.Add(o => !o.IS_GOODS_RESTRICT.HasValue || o.IS_GOODS_RESTRICT.Value != ManagerConstant.IS_TRUE);
                }

                search.listVHisMedicineBeanExpression.AddRange(listVHisMedicineBeanExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicineBeanExpression.Clear();
                search.listVHisMedicineBeanExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
