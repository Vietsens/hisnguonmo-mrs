using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBidBloodTypeSO : StagingObjectBase
    {
        public HisBidBloodTypeSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BID_BLOOD_TYPE, bool>>> listHisBidBloodTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BID_BLOOD_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BID_BLOOD_TYPE, bool>>> listVHisBidBloodTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID_BLOOD_TYPE, bool>>>();
    }
}
