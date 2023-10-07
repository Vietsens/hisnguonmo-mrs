using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccidentCareSO : StagingObjectBase
    {
        public HisAccidentCareSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_CARE, bool>>> listHisAccidentCareExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_CARE, bool>>>();
    }
}
