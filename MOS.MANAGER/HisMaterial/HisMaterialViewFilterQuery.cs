using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterial
{
    public class HisMaterialViewFilterQuery : HisMaterialViewFilter
    {
        public HisMaterialViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL, bool>>> listVHisMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL, bool>>>();

        

        internal HisMaterialSO Query()
        {
            HisMaterialSO search = new HisMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisMaterialExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisMaterialExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisMaterialExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisMaterialExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.BID_NUMBER__EXACT))
                {
                    listVHisMaterialExpression.Add(o => o.TDL_BID_NUMBER == this.BID_NUMBER__EXACT);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMaterialExpression.Add(o => o.TDL_BID_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.MATERIAL_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.MATERIAL_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.SERVICE_UNIT_SYMBOL.ToLower().Contains(this.KEY_WORD)
                        || o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.HEIN_SERVICE_BHYT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.HEIN_SERVICE_BHYT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (this.BID_ID.HasValue)
                {
                    listVHisMaterialExpression.Add(o => o.BID_ID.HasValue && o.BID_ID.Value == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisMaterialExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }

                search.listVHisMaterialExpression.AddRange(listVHisMaterialExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMaterialExpression.Clear();
                search.listVHisMaterialExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
