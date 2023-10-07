using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceMatySO : StagingObjectBase
    {
        public HisServiceMatySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MATY, bool>>> listHisServiceMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_MATY, bool>>> listVHisServiceMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_MATY, bool>>>();
    }
}
