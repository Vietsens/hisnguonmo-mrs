using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServMatySO : StagingObjectBase
    {
        public HisSereServMatySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_MATY, bool>>> listHisSereServMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_MATY, bool>>> listVHisSereServMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_MATY, bool>>>();
    }
}
