using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentVehicleSO : StagingObjectBase
    {
        public HisAccidentVehicleSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_VEHICLE, bool>>> listHisAccidentVehicleExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_VEHICLE, bool>>>();
    }
}
