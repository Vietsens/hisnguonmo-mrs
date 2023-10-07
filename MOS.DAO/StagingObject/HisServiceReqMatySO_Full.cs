using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceReqMatySO : StagingObjectBase
    {
        public HisServiceReqMatySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_MATY, bool>>> listHisServiceReqMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ_MATY, bool>>>();
    }
}
