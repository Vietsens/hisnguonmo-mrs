using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRoom
{
    public class HisServiceRoomViewFilterQuery : HisServiceRoomViewFilter
    {
        public HisServiceRoomViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_ROOM, bool>>> listVHisServiceRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_ROOM, bool>>>();

        

        internal HisServiceRoomSO Query()
        {
            HisServiceRoomSO search = new HisServiceRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisServiceRoomExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisServiceRoomExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisServiceRoomExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisServiceRoomExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisServiceRoomExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.SERVICE_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_TYPE_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.SERVICE_TYPE_ID == this.SERVICE_TYPE_ID.Value);
                }
                if (this.PARENT_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.PARENT_ID.Value);
                }
                if (this.SERVICE_IDS != null)
                {
                    search.listVHisServiceRoomExpression.Add(o => this.SERVICE_IDS.Contains(o.SERVICE_ID));
                }
                if (this.SERVICE_TYPE_IDs != null)
                {
                    search.listVHisServiceRoomExpression.Add(o => this.SERVICE_TYPE_IDs.Contains(o.SERVICE_TYPE_ID));
                }
                if (this.ROOM_TYPE_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.ROOM_TYPE_ID == this.ROOM_TYPE_ID.Value);
                }
                if (this.ROOM_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.IS_LEAF.HasValue && this.IS_LEAF.Value)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.IS_LEAF.HasValue && o.IS_LEAF.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_LEAF.HasValue && !this.IS_LEAF.Value)
                {
                    search.listVHisServiceRoomExpression.Add(o => !o.IS_LEAF.HasValue || o.IS_LEAF.Value != ManagerConstant.IS_TRUE);
                }
                if (this.BRANCH_ID.HasValue)
                {
                    search.listVHisServiceRoomExpression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }
                if (this.BRANCH_IDs != null)
                {
                    search.listVHisServiceRoomExpression.Add(o => this.BRANCH_IDs.Contains(o.BRANCH_ID));
                }

                search.listVHisServiceRoomExpression.AddRange(listVHisServiceRoomExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisServiceRoomExpression.Clear();
                search.listVHisServiceRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
