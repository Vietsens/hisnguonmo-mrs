using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisReceptionRoomSO : StagingObjectBase
    {
        public HisReceptionRoomSO()
        {
            //listHisReceptionRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisReceptionRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_RECEPTION_ROOM, bool>>> listHisReceptionRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_RECEPTION_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_RECEPTION_ROOM, bool>>> listVHisReceptionRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_RECEPTION_ROOM, bool>>>();
    }
}
