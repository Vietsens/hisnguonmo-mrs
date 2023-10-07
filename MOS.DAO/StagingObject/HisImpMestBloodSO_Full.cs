using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestBloodSO : StagingObjectBase
    {
        public HisImpMestBloodSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_BLOOD, bool>>> listHisImpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_BLOOD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_BLOOD, bool>>> listVHisImpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_BLOOD, bool>>>();
    }
}
