using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBloodTypeSO : StagingObjectBase
    {
        public HisBloodTypeSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_TYPE, bool>>> listHisBloodTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD_TYPE, bool>>> listVHisBloodTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD_TYPE, bool>>>();
    }
}
