using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAnticipateMatySO : StagingObjectBase
    {
        public HisAnticipateMatySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_MATY, bool>>> listHisAnticipateMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_MATY, bool>>> listVHisAnticipateMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_MATY, bool>>>();
    }
}
