using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterial
{
    public class HisMaterialView1FilterQuery : HisMaterialView1Filter
    {
        public HisMaterialView1FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_1, bool>>> listVHisMaterial1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_1, bool>>>();

        internal HisMaterialSO Query()
        {
            HisMaterialSO search = new HisMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisMaterial1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisMaterial1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisMaterial1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisMaterial1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisMaterial1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.BID_NUMBER__EXACT))
                {
                    listVHisMaterial1Expression.Add(o => o.TDL_BID_NUMBER == this.BID_NUMBER__EXACT);
                }
                if (!String.IsNullOrEmpty(this.MATERIAL_TYPE_CODE__EXACT))
                {
                    listVHisMaterial1Expression.Add(o => o.MATERIAL_TYPE_CODE == this.MATERIAL_TYPE_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMaterial1Expression.Add(o => o.TDL_BID_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.MATERIAL_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.MATERIAL_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (this.BID_ID.HasValue)
                {
                    listVHisMaterial1Expression.Add(o => o.BID_ID.HasValue && o.BID_ID.Value == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisMaterial1Expression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisMaterial1Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisMaterial1Expression.Add(o => o.IMP_TIME.HasValue && o.IMP_TIME.Value <= this.IMP_TIME_TO.Value);
                }

                search.listVHisMaterial1Expression.AddRange(listVHisMaterial1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMaterial1Expression.Clear();
                search.listVHisMaterial1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
