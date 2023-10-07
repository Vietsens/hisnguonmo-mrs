using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSampleRoomSO : StagingObjectBase
    {
        public HisSampleRoomSO()
        {
            //listHisSampleRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSampleRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SAMPLE_ROOM, bool>>> listHisSampleRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SAMPLE_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SAMPLE_ROOM, bool>>> listVHisSampleRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SAMPLE_ROOM, bool>>>();
    }
}
