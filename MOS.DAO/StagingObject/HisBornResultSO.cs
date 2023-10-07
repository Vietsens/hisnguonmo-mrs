using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBornResultSO : StagingObjectBase
    {
        public HisBornResultSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BORN_RESULT, bool>>> listHisBornResultExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BORN_RESULT, bool>>>();
    }
}
