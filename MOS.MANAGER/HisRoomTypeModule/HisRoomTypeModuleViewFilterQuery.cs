using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomTypeModule
{
    public class HisRoomTypeModuleViewFilterQuery : HisRoomTypeModuleViewFilter
    {
        public HisRoomTypeModuleViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_TYPE_MODULE, bool>>> listVHisRoomTypeModuleExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_TYPE_MODULE, bool>>>();



        internal HisRoomTypeModuleSO Query()
        {
            HisRoomTypeModuleSO search = new HisRoomTypeModuleSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisRoomTypeModuleExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.ROOM_TYPE_ID.HasValue)
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.ROOM_TYPE_ID == this.ROOM_TYPE_ID.Value);
                }
                if (this.ROOM_TYPE_IDs != null)
                {
                    listVHisRoomTypeModuleExpression.Add(o => this.ROOM_TYPE_IDs.Contains(o.ROOM_TYPE_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisRoomTypeModuleExpression.Add(o =>
                        o.MODULE_LINK.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.MODULE_LINK__EXACT))
                {
                    listVHisRoomTypeModuleExpression.Add(o => o.MODULE_LINK == this.MODULE_LINK__EXACT);
                }
                if (this.MODULE_LINK__EXACTs != null)
                {
                    listVHisRoomTypeModuleExpression.Add(o => this.MODULE_LINK__EXACTs.Contains(o.MODULE_LINK));
                }

                search.listVHisRoomTypeModuleExpression.AddRange(listVHisRoomTypeModuleExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisRoomTypeModuleExpression.Clear();
                search.listVHisRoomTypeModuleExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
