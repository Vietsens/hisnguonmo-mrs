using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRemunerationSO : StagingObjectBase
    {
        public HisRemunerationSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REMUNERATION, bool>>> listHisRemunerationExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REMUNERATION, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_REMUNERATION, bool>>> listVHisRemunerationExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REMUNERATION, bool>>>();
    }
}
