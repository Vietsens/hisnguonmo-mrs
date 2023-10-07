using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicine
{
    public class HisMedicineView2FilterQuery : HisMedicineView2Filter
    {
        public HisMedicineView2FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_2, bool>>> listVHisMedicine2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_2, bool>>>();

        

        internal HisMedicineSO Query()
        {
            HisMedicineSO search = new HisMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicine2Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicine2Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicine2Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicine2Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicine2Expression.Add(o =>
                        o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.MEDICINE_TYPE_CODE__EXACT))
                {
                    listVHisMedicine2Expression.Add(o => o.MEDICINE_TYPE_CODE == this.MEDICINE_TYPE_CODE__EXACT);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisMedicine2Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }

                search.listVHisMedicine2Expression.AddRange(listVHisMedicine2Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicine2Expression.Clear();
                search.listVHisMedicine2Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
