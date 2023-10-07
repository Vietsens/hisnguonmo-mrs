using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisUserRoomSO : StagingObjectBase
    {
        public HisUserRoomSO()
        {
            //listHisUserRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisUserRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_USER_ROOM, bool>>> listHisUserRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_USER_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_USER_ROOM, bool>>> listVHisUserRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_USER_ROOM, bool>>>();
    }
}
