using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRoomSO : StagingObjectBase
    {
        public HisRoomSO()
        {
            //listHisRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisRoomCounterExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ROOM, bool>>> listHisRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM, bool>>> listVHisRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER, bool>>> listVHisRoomCounterExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER_1, bool>>> listVHisRoomCounter1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_COUNTER_1, bool>>>();
    }
}
