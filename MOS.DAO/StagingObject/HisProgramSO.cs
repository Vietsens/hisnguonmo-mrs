using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisProgramSO : StagingObjectBase
    {
        public HisProgramSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PROGRAM, bool>>> listHisProgramExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PROGRAM, bool>>>();
    }
}
