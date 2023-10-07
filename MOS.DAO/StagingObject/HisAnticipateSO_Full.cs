using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAnticipateSO : StagingObjectBase
    {
        public HisAnticipateSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE, bool>>> listHisAnticipateExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE, bool>>> listVHisAnticipateExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE, bool>>>();
    }
}
