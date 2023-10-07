using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisIcdServiceSO : StagingObjectBase
    {
        public HisIcdServiceSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ICD_SERVICE, bool>>> listHisIcdServiceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ICD_SERVICE, bool>>>();
    }
}
