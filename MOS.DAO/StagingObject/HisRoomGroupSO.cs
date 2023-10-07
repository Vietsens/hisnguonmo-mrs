using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRoomGroupSO : StagingObjectBase
    {
        public HisRoomGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ROOM_GROUP, bool>>> listHisRoomGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ROOM_GROUP, bool>>>();
    }
}
