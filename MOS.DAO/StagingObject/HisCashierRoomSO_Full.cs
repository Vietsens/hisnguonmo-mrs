using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCashierRoomSO : StagingObjectBase
    {
        public HisCashierRoomSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CASHIER_ROOM, bool>>> listHisCashierRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CASHIER_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_CASHIER_ROOM, bool>>> listVHisCashierRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CASHIER_ROOM, bool>>>();
    }
}
