using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRoom
{
    public class HisRoomCounter1ViewFilterQuery : HisRoomCounter1ViewFilter
    {
        public HisRoomCounter1ViewFilterQuery()
            : base()
        {

        }


        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER_1, bool>>> listVHisRoomCounter1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER_1, bool>>>();

        internal HisRoomSO Query()
        {
            HisRoomSO search = new HisRoomSO();
            try
            {
                if (this.ID.HasValue)
                {
                    search.listVHisRoomCounter1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisRoomCounter1Expression.Add(o => this.IDs.Contains(o.ID));
                }

                search.listVHisRoomCounter1Expression.AddRange(listVHisRoomCounter1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisRoomExpression.Clear();
                search.listVHisRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
