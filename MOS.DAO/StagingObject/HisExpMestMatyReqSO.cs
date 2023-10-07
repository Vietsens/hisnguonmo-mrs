using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestMatyReqSO : StagingObjectBase
    {
        public HisExpMestMatyReqSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATY_REQ, bool>>> listHisExpMestMatyReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATY_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATY_REQ, bool>>> listVHisExpMestMatyReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATY_REQ, bool>>>();

    }
}
