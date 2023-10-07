using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServFileSO : StagingObjectBase
    {
        public HisSereServFileSO()
        {
            //listHisSereServFileExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_FILE, bool>>> listHisSereServFileExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_FILE, bool>>>();
    }
}
