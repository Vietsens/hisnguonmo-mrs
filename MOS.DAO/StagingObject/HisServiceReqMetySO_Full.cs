using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceReqMetySO : StagingObjectBase
    {
        public HisServiceReqMetySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_METY, bool>>> listHisServiceReqMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_METY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_METY, bool>>> listVHisServiceReqMetyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_METY, bool>>>();
    }
}
