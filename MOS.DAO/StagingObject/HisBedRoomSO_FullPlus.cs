using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBedRoomSO : StagingObjectBase
    {
        public HisBedRoomSO()
        {
            //listHisBedRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisBedRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BED_ROOM, bool>>> listHisBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BED_ROOM, bool>>> listVHisBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BED_ROOM, bool>>>();
    }
}
