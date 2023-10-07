using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisInfusionSumSO : StagingObjectBase
    {
        public HisInfusionSumSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_INFUSION_SUM, bool>>> listHisInfusionSumExpression = new List<System.Linq.Expressions.Expression<Func<HIS_INFUSION_SUM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION_SUM, bool>>> listVHisInfusionSumExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_INFUSION_SUM, bool>>>();
    }
}
