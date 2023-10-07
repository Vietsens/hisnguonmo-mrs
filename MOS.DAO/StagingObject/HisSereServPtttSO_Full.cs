using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServPtttSO : StagingObjectBase
    {
        public HisSereServPtttSO()
        {
            //listHisSereServPtttExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisSereServPtttExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_PTTT, bool>>> listHisSereServPtttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_PTTT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_PTTT, bool>>> listVHisSereServPtttExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_PTTT, bool>>>();
    }
}
