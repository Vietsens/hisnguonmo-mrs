using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisOweTypeSO : StagingObjectBase
    {
        public HisOweTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_OWE_TYPE, bool>>> listHisOweTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_OWE_TYPE, bool>>>();
    }
}
