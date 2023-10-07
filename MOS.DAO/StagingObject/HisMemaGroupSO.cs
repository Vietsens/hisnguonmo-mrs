using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMemaGroupSO : StagingObjectBase
    {
        public HisMemaGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEMA_GROUP, bool>>> listHisMemaGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEMA_GROUP, bool>>>();
    }
}
