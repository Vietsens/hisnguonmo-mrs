using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEquipmentSetMatySO : StagingObjectBase
    {
        public HisEquipmentSetMatySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EQUIPMENT_SET_MATY, bool>>> listHisEquipmentSetMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EQUIPMENT_SET_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EQUIPMENT_SET_MATY, bool>>> listVHisEquipmentSetMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EQUIPMENT_SET_MATY, bool>>>();
    }
}
