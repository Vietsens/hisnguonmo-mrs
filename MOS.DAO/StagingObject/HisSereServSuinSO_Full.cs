using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServSuinSO : StagingObjectBase
    {
        public HisSereServSuinSO()
        {
            //listHisSereServSuinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSereServSuinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_SUIN, bool>>> listHisSereServSuinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_SUIN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_SUIN, bool>>> listVHisSereServSuinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_SUIN, bool>>>();
    }
}
