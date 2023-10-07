using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineType
{
    public class HisMedicineTypeViewFilterQuery : HisMedicineTypeViewFilter
    {
        public HisMedicineTypeViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE, bool>>> listVHisMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE, bool>>>();

        

        internal HisMedicineTypeSO Query()
        {
            HisMedicineTypeSO search = new HisMedicineTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisMedicineTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.IS_LEAF.HasValue && this.IS_LEAF.Value)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.IS_LEAF.HasValue && o.IS_LEAF.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_LEAF.HasValue && !this.IS_LEAF.Value)
                {
                    search.listVHisMedicineTypeExpression.Add(o => !o.IS_LEAF.HasValue || o.IS_LEAF.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_STOP_IMP.HasValue && this.IS_STOP_IMP.Value)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.IS_STOP_IMP.HasValue && o.IS_STOP_IMP.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_STOP_IMP.HasValue && !this.IS_STOP_IMP.Value)
                {
                    search.listVHisMedicineTypeExpression.Add(o => !o.IS_STOP_IMP.HasValue || o.IS_STOP_IMP.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IDs != null)
                {
                    search.listVHisMedicineTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }

                if (this.IsTree.HasValue && this.IsTree.Value)
                {
                    listVHisMedicineTypeExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
                }
                if (!String.IsNullOrEmpty(this.CN_WORD))
                {
                    this.CN_WORD = this.CN_WORD.ToLower().Trim();
                    listVHisMedicineTypeExpression.Add(o =>
                        o.MEDICINE_TYPE_CODE.ToLower().Contains(this.CN_WORD) ||
                        o.MEDICINE_TYPE_NAME.ToLower().Contains(this.CN_WORD));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisMedicineTypeExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_PROPRIETARY_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TCY_NUM_ORDER.ToLower().Contains(this.KEY_WORD) ||
                        o.TUTORIAL.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.MANUFACTURER_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MANUFACTURER_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.SERVICE_ID.HasValue)
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    search.listVHisMedicineTypeExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (!String.IsNullOrEmpty(this.NATIONAL_NAME__EXACT))
                {
                    search.listVHisMedicineTypeExpression.Add(o => o.NATIONAL_NAME == this.NATIONAL_NAME__EXACT);
                }
                if (this.IS_MISS_BHYT_INFO.HasValue && this.IS_MISS_BHYT_INFO.Value)
                {
                    listVHisMedicineTypeExpression.Add(o =>
                        o.ACTIVE_INGR_BHYT_CODE == null || o.ACTIVE_INGR_BHYT_CODE.Trim() == ""
                        || o.ACTIVE_INGR_BHYT_NAME == null || o.ACTIVE_INGR_BHYT_NAME.Trim() == ""
                        || o.REGISTER_NUMBER == null || o.REGISTER_NUMBER.Trim() == ""
                        || !o.MEDICINE_USE_FORM_ID.HasValue
                        || o.CONCENTRA == null || o.CONCENTRA.Trim() == ""
                        || !o.HEIN_SERVICE_TYPE_ID.HasValue);
                }
                if (this.IS_MISS_BHYT_INFO.HasValue && !this.IS_MISS_BHYT_INFO.Value)
                {
                    listVHisMedicineTypeExpression.Add(o =>
                        o.ACTIVE_INGR_BHYT_CODE != null
                        && o.ACTIVE_INGR_BHYT_NAME != null
                        && o.REGISTER_NUMBER != null
                        && o.MEDICINE_USE_FORM_ID.HasValue
                        && o.CONCENTRA != null
                        && o.HEIN_SERVICE_TYPE_ID.HasValue);
                }

                search.listVHisMedicineTypeExpression.AddRange(listVHisMedicineTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMedicineTypeExpression.Clear();
                search.listVHisMedicineTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
