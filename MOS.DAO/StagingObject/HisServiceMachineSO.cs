using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceMachineSO : StagingObjectBase
    {
        public HisServiceMachineSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MACHINE, bool>>> listHisServiceMachineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MACHINE, bool>>>();
    }
}
