using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicine
{
    public class HisMedicineView1FilterQuery : HisMedicineView1Filter
    {
        public HisMedicineView1FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_1, bool>>> listVHisMedicine1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_1, bool>>>();

        

        internal HisMedicineSO Query()
        {
            HisMedicineSO search = new HisMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicine1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicine1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicine1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicine1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicine1Expression.Add(o => o.TDL_BID_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.MEDICINE_TYPE_CODE__EXACT))
                {
                    listVHisMedicine1Expression.Add(o => o.MEDICINE_TYPE_CODE == this.MEDICINE_TYPE_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.BID_NUMBER__EXACT))
                {
                    listVHisMedicine1Expression.Add(o => o.TDL_BID_NUMBER == this.BID_NUMBER__EXACT);
                }
                if (this.BID_ID.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.BID_ID.HasValue && o.BID_ID.Value == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisMedicine1Expression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisMedicine1Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }
                if (this.IS_MISS_BHYT_INFO.HasValue && this.IS_MISS_BHYT_INFO.Value)
                {
                    listVHisMedicine1Expression.Add(o =>
                        o.ACTIVE_INGR_BHYT_CODE == null || o.ACTIVE_INGR_BHYT_CODE.Trim() == ""
                        || o.ACTIVE_INGR_BHYT_NAME == null || o.ACTIVE_INGR_BHYT_NAME.Trim() == ""
                        || o.MEDICINE_REGISTER_NUMBER == null || o.MEDICINE_REGISTER_NUMBER.Trim() == ""
                        || !o.MEDICINE_USE_FORM_ID.HasValue
                        || o.CONCENTRA == null || o.CONCENTRA.Trim() == "");
                }
                if (this.IS_MISS_BHYT_INFO.HasValue && !this.IS_MISS_BHYT_INFO.Value)
                {
                    listVHisMedicine1Expression.Add(o =>
                        o.ACTIVE_INGR_BHYT_CODE != null
                        && o.ACTIVE_INGR_BHYT_NAME != null
                        && o.MEDICINE_REGISTER_NUMBER != null
                        && o.MEDICINE_USE_FORM_ID.HasValue
                        && o.CONCENTRA != null);
                }

                search.listVHisMedicine1Expression.AddRange(listVHisMedicine1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicine1Expression.Clear();
                search.listVHisMedicine1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
