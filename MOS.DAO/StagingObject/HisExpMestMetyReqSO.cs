using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestMetyReqSO : StagingObjectBase
    {
        public HisExpMestMetyReqSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_METY_REQ, bool>>> listHisExpMestMetyReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_METY_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ, bool>>> listVHisExpMestMetyReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ_1, bool>>> listVHisExpMestMetyReq1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_METY_REQ_1, bool>>>();
    }
}
