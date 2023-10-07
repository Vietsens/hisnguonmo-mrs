using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceMetySO : StagingObjectBase
    {
        public HisServiceMetySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_METY, bool>>> listHisServiceMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_METY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_METY, bool>>> listVHisServiceMetyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_METY, bool>>>();
    }
}
