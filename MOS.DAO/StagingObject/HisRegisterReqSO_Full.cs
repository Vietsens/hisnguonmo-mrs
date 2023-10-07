using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRegisterReqSO : StagingObjectBase
    {
        public HisRegisterReqSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_REQ, bool>>> listHisRegisterReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_REGISTER_REQ, bool>>> listVHisRegisterReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REGISTER_REQ, bool>>>();
    }
}
