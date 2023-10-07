using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestMetyDepaSO : StagingObjectBase
    {
        public HisMestMetyDepaSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_METY_DEPA, bool>>> listHisMestMetyDepaExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_METY_DEPA, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_METY_DEPA, bool>>> listVHisMestMetyDepaExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_METY_DEPA, bool>>>();
    }
}
