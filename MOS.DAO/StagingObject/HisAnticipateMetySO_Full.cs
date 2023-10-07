using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAnticipateMetySO : StagingObjectBase
    {
        public HisAnticipateMetySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_METY, bool>>> listHisAnticipateMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_METY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_METY, bool>>> listVHisAnticipateMetyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_METY, bool>>>();
    }
}
