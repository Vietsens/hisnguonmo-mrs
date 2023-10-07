using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestRoomSO : StagingObjectBase
    {
        public HisMestRoomSO()
        {
            //listHisMestRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_ROOM, bool>>> listHisMestRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_ROOM, bool>>> listVHisMestRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_ROOM, bool>>>();
    }
}
