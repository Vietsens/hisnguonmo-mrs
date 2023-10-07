using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    public class HisExecuteRoomFilterQuery : HisExecuteRoomFilter
    {
        public HisExecuteRoomFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROOM, bool>>> listHisExecuteRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROOM, bool>>>();

        

        internal HisExecuteRoomSO Query()
        {
            HisExecuteRoomSO search = new HisExecuteRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisExecuteRoomExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisExecuteRoomExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisExecuteRoomExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisExecuteRoomExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.ROOM_ID.HasValue)
                {
                    search.listHisExecuteRoomExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.IS_EMERGENCY.HasValue)
                {
                    if (this.IS_EMERGENCY.Value)
                    {
                        search.listHisExecuteRoomExpression.Add(o => o.IS_EMERGENCY.HasValue && o.IS_EMERGENCY == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        search.listHisExecuteRoomExpression.Add(o => !o.IS_EMERGENCY.HasValue || o.IS_EMERGENCY != ManagerConstant.IS_TRUE);
                    }
                }
                #endregion

                if (this.ROOM_ID.HasValue)
                {
                    listHisExecuteRoomExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.ROOM_IDs != null)
                {
                    listHisExecuteRoomExpression.Add(o => this.ROOM_IDs.Contains(o.ROOM_ID));
                }
                if (this.IS_EMERGENCY.HasValue && this.IS_EMERGENCY.Value)
                {
                    listHisExecuteRoomExpression.Add(o => o.IS_EMERGENCY.HasValue && o.IS_EMERGENCY.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EMERGENCY.HasValue && !this.IS_EMERGENCY.Value)
                {
                    listHisExecuteRoomExpression.Add(o => !o.IS_EMERGENCY.HasValue || o.IS_EMERGENCY.Value != ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXAM.HasValue && this.IS_EXAM.Value)
                {
                    listHisExecuteRoomExpression.Add(o => o.IS_EXAM == ManagerConstant.IS_TRUE);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisExecuteRoomExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisExecuteRoomExpression.AddRange(listHisExecuteRoomExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExecuteRoomExpression.Clear();
                search.listHisExecuteRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
