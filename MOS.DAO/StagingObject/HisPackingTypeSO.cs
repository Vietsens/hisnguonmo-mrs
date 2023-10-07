using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPackingTypeSO : StagingObjectBase
    {
        public HisPackingTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PACKING_TYPE, bool>>> listHisPackingTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PACKING_TYPE, bool>>>();
    }
}
