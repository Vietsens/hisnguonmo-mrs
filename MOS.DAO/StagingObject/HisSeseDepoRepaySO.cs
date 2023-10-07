using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSeseDepoRepaySO : StagingObjectBase
    {
        public HisSeseDepoRepaySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SESE_DEPO_REPAY, bool>>> listHisSeseDepoRepayExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SESE_DEPO_REPAY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SESE_DEPO_REPAY, bool>>> listVHisSeseDepoRepayExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SESE_DEPO_REPAY, bool>>>();
    }
}
