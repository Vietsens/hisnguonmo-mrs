using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public class HisMedicineBeanView1FilterQuery : HisMedicineBeanView1Filter
    {
        public HisMedicineBeanView1FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_1, bool>>> listVHisMedicineBean1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_1, bool>>>();

        

        internal HisMedicineBeanSO Query()
        {
            HisMedicineBeanSO search = new HisMedicineBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicineBean1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicineBean1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicineBean1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicineBean1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID);
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID);
                }
                if (this.MEDICINE_TYPE_IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MEDICINE_TYPE_IS_ACTIVE.HasValue && o.MEDICINE_TYPE_IS_ACTIVE.Value == this.MEDICINE_TYPE_IS_ACTIVE.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisMedicineBean1Expression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisMedicineBean1Expression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.SOURCE_IDs != null)
                {
                    listVHisMedicineBean1Expression.Add(o => o.SOURCE_ID != null && this.SOURCE_IDs.Contains(o.SOURCE_ID.Value));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    search.listVHisMedicineBean1Expression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicineBean1Expression.Add(o =>
                        o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.MEDICINE_IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBean1Expression.Add(o => o.MEDICINE_IS_ACTIVE.HasValue && o.MEDICINE_IS_ACTIVE.Value == this.MEDICINE_IS_ACTIVE.Value);
                }

                search.listVHisMedicineBean1Expression.AddRange(listVHisMedicineBean1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicineBean1Expression.Clear();
                search.listVHisMedicineBean1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
