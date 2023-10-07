using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPtttGroupSO : StagingObjectBase
    {
        public HisPtttGroupSO()
        {
            //listHisPtttGroupExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PTTT_GROUP, bool>>> listHisPtttGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PTTT_GROUP, bool>>>();
    }
}
