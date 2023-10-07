using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCareTypeSO : StagingObjectBase
    {
        public HisCareTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CARE_TYPE, bool>>> listHisCareTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARE_TYPE, bool>>>();
    }
}
