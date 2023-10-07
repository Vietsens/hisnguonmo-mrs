using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisVitaminASO : StagingObjectBase
    {
        public HisVitaminASO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_VITAMIN_A, bool>>> listHisVitaminAExpression = new List<System.Linq.Expressions.Expression<Func<HIS_VITAMIN_A, bool>>>();
    }
}
