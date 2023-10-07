using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSetMaty
{
    public class HisEquipmentSetMatyViewFilterQuery : HisEquipmentSetMatyViewFilter
    {
        public HisEquipmentSetMatyViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EQUIPMENT_SET_MATY, bool>>> listVHisEquipmentSetMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EQUIPMENT_SET_MATY, bool>>>();



        internal HisEquipmentSetMatySO Query()
        {
            HisEquipmentSetMatySO search = new HisEquipmentSetMatySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EQUIPMENT_SET_ID.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.EQUIPMENT_SET_ID == this.EQUIPMENT_SET_ID.Value);
                }
                if (this.EQUIPMENT_SET_IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.EQUIPMENT_SET_IDs.Contains(o.EQUIPMENT_SET_ID));
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID));
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.SERVICE_UNIT_ID.HasValue)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.SERVICE_UNIT_ID == this.SERVICE_UNIT_ID.Value);
                }
                if (this.SERVICE_UNIT_IDs != null)
                {
                    listVHisEquipmentSetMatyExpression.Add(o => this.SERVICE_UNIT_IDs.Contains(o.SERVICE_UNIT_ID));
                }
                if (!String.IsNullOrEmpty(this.MATERIAL_TYPE_CODE__EXACT))
                {
                    listVHisEquipmentSetMatyExpression.Add(o => o.MATERIAL_TYPE_CODE == this.MATERIAL_TYPE_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.Trim();
                    listVHisEquipmentSetMatyExpression.Add(o => o.APP_CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.GROUP_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.NATIONAL_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisEquipmentSetMatyExpression.AddRange(listVHisEquipmentSetMatyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisEquipmentSetMatyExpression.Clear();
                search.listVHisEquipmentSetMatyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
