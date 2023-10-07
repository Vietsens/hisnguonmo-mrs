using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCareDetailSO : StagingObjectBase
    {
        public HisCareDetailSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CARE_DETAIL, bool>>> listHisCareDetailExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARE_DETAIL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_CARE_DETAIL, bool>>> listVHisCareDetailExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CARE_DETAIL, bool>>>();
    }
}
