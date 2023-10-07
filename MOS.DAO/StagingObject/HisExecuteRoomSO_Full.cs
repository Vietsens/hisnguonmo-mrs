using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExecuteRoomSO : StagingObjectBase
    {
        public HisExecuteRoomSO()
        {
            //listHisExecuteRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisExecuteRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROOM, bool>>> listHisExecuteRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXECUTE_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM, bool>>> listVHisExecuteRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM_1, bool>>> listVHisExecuteRoom1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM_1, bool>>>();
    }
}
