using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public class HisMedicineBeanView2FilterQuery : HisMedicineBeanView2Filter
    {
        public HisMedicineBeanView2FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_2, bool>>> listVHisMedicineBean2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_2, bool>>>();

        

        internal HisMedicineBeanSO Query()
        {
            HisMedicineBeanSO search = new HisMedicineBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicineBean2Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicineBean2Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicineBean2Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicineBean2Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID);
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisMedicineBean2Expression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisMedicineBean2Expression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.SOURCE_IDs != null)
                {
                    listVHisMedicineBean2Expression.Add(o => o.SOURCE_ID != null && this.SOURCE_IDs.Contains(o.SOURCE_ID.Value));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    search.listVHisMedicineBean2Expression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID);
                }
                if (this.MEDICINE_IS_ACTIVE.HasValue)
                {
                    listVHisMedicineBean2Expression.Add(o => o.MEDICINE_IS_ACTIVE.HasValue && o.MEDICINE_IS_ACTIVE.Value == this.MEDICINE_IS_ACTIVE.Value);
                }

                search.listVHisMedicineBean2Expression.AddRange(listVHisMedicineBean2Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicineBean2Expression.Clear();
                search.listVHisMedicineBean2Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
