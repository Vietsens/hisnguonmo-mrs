using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInfusionSO : StagingObjectBase
    {
        public HisInfusionSO()
        {
            //listHisInfusionExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisInfusionExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INFUSION, bool>>> listHisInfusionExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INFUSION, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION, bool>>> listVHisInfusionExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION, bool>>>();
    }
}
