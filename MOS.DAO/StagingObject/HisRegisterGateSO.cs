using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRegisterGateSO : StagingObjectBase
    {
        public HisRegisterGateSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_GATE, bool>>> listHisRegisterGateExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_GATE, bool>>>();
    }
}
