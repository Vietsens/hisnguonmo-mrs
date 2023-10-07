using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPatySubSO : StagingObjectBase
    {
        public HisMestPatySubSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PATY_SUB, bool>>> listHisMestPatySubExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PATY_SUB, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PATY_SUB, bool>>> listVHisMestPatySubExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PATY_SUB, bool>>>();
    }
}
