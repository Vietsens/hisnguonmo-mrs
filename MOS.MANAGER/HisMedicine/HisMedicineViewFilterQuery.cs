using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicine
{
    public class HisMedicineViewFilterQuery : HisMedicineViewFilter
    {
        public HisMedicineViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE, bool>>> listVHisMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE, bool>>>();

        

        internal HisMedicineSO Query()
        {
            HisMedicineSO search = new HisMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMedicineExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMedicineExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMedicineExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMedicineExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicineExpression.Add(o => o.TDL_BID_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.REGISTER_NUMBER.ToLower().Contains(this.KEY_WORD)
                        || o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD)
                        || o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.BID_NUMBER__EXACT))
                {
                    listVHisMedicineExpression.Add(o => o.TDL_BID_NUMBER == this.BID_NUMBER__EXACT);
                }
                if (this.BID_ID.HasValue)
                {
                    listVHisMedicineExpression.Add(o => o.BID_ID.HasValue && o.BID_ID.Value == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisMedicineExpression.Add(o => o.BID_ID.HasValue && this.BID_IDs.Contains(o.BID_ID.Value));
                }
                
                search.listVHisMedicineExpression.AddRange(listVHisMedicineExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicineExpression.Clear();
                search.listVHisMedicineExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
