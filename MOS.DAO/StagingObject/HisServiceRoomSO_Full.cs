using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceRoomSO : StagingObjectBase
    {
        public HisServiceRoomSO()
        {
            //listHisServiceRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisServiceRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_ROOM, bool>>> listHisServiceRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_ROOM, bool>>> listVHisServiceRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_ROOM, bool>>>();
    }
}
