using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoomTypeModule
{
    public class HisRoomTypeModuleFilterQuery : HisRoomTypeModuleFilter
    {
        public HisRoomTypeModuleFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ROOM_TYPE_MODULE, bool>>> listHisRoomTypeModuleExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ROOM_TYPE_MODULE, bool>>>();

        

        internal HisRoomTypeModuleSO Query()
        {
            HisRoomTypeModuleSO search = new HisRoomTypeModuleSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisRoomTypeModuleExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisRoomTypeModuleExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisRoomTypeModuleExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisRoomTypeModuleExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisRoomTypeModuleExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.ROOM_TYPE_ID.HasValue)
                {
                    listHisRoomTypeModuleExpression.Add(o => o.ROOM_TYPE_ID == this.ROOM_TYPE_ID.Value);
                }
                if (this.ROOM_TYPE_IDs != null)
                {
                    listHisRoomTypeModuleExpression.Add(o => this.ROOM_TYPE_IDs.Contains(o.ROOM_TYPE_ID));
                }
                if (!String.IsNullOrEmpty(this.MODULE_LINK__EXACT))
                {
                    listHisRoomTypeModuleExpression.Add(o => o.MODULE_LINK == this.MODULE_LINK__EXACT);
                }
                if (this.MODULE_LINK__EXACTs != null)
                {
                    listHisRoomTypeModuleExpression.Add(o => this.MODULE_LINK__EXACTs.Contains(o.MODULE_LINK));
                }

                search.listHisRoomTypeModuleExpression.AddRange(listHisRoomTypeModuleExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisRoomTypeModuleExpression.Clear();
                search.listHisRoomTypeModuleExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
