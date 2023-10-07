using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBloodSO : StagingObjectBase
    {
        public HisBloodSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BLOOD, bool>>> listHisBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD, bool>>> listVHisBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD, bool>>>();
    }
}
