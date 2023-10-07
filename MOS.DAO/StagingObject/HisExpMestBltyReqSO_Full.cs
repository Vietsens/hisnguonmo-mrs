using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestBltyReqSO : StagingObjectBase
    {
        public HisExpMestBltyReqSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLTY_REQ, bool>>> listHisExpMestBltyReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLTY_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLTY_REQ, bool>>> listVHisExpMestBltyReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLTY_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLTY_REQ_1, bool>>> listVHisExpMestBltyReq1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLTY_REQ_1, bool>>>();
    }
}
