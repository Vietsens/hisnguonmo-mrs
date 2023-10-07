using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestInventorySO : StagingObjectBase
    {
        public HisMestInventorySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVENTORY, bool>>> listHisMestInventoryExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_INVENTORY, bool>>>();
    }
}
