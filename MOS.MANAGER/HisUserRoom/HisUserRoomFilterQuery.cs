using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserRoom
{
    public class HisUserRoomFilterQuery : HisUserRoomFilter
    {
        public HisUserRoomFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_USER_ROOM, bool>>> listHisUserRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_USER_ROOM, bool>>>();

        

        internal HisUserRoomSO Query()
        {
            HisUserRoomSO search = new HisUserRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisUserRoomExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisUserRoomExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisUserRoomExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisUserRoomExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion
                
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    listHisUserRoomExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.LOGINNAME.ToLower().Contains(this.KEY_WORD)
                        );
                    this.KEY_WORD = this.KEY_WORD.Trim().ToLower();
                }

                if (!String.IsNullOrEmpty(this.LOGINNAME__EXACT))
                {
                    listHisUserRoomExpression.Add(o => this.LOGINNAME__EXACT.Equals(o.LOGINNAME));
                }
                if (this.ROOM_ID.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    listHisUserRoomExpression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }
                
                search.listHisUserRoomExpression.AddRange(listHisUserRoomExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisUserRoomExpression.Clear();
                listHisUserRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
