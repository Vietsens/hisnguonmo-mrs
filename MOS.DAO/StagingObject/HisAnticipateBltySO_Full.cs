using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAnticipateBltySO : StagingObjectBase
    {
        public HisAnticipateBltySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_BLTY, bool>>> listHisAnticipateBltyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ANTICIPATE_BLTY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_BLTY, bool>>> listVHisAnticipateBltyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE_BLTY, bool>>>();
    }
}
