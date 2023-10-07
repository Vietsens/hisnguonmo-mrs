using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisWorkPlaceSO : StagingObjectBase
    {
        public HisWorkPlaceSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_WORK_PLACE, bool>>> listHisWorkPlaceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_WORK_PLACE, bool>>>();
    }
}
