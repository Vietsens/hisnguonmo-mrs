using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRoomTypeModuleSO : StagingObjectBase
    {
        public HisRoomTypeModuleSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ROOM_TYPE_MODULE, bool>>> listHisRoomTypeModuleExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ROOM_TYPE_MODULE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_TYPE_MODULE, bool>>> listVHisRoomTypeModuleExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ROOM_TYPE_MODULE, bool>>>();
    }
}
