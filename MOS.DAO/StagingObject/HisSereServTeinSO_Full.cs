using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServTeinSO : StagingObjectBase
    {
        public HisSereServTeinSO()
        {
            //listHisSereServTeinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSereServTeinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEIN, bool>>> listHisSereServTeinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEIN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_TEIN, bool>>> listVHisSereServTeinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_TEIN, bool>>>();
    }
}
