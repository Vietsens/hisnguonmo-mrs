using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTranPatiReasonSO : StagingObjectBase
    {
        public HisTranPatiReasonSO()
        {
            //listHisTranPatiReasonExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TRAN_PATI_REASON, bool>>> listHisTranPatiReasonExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TRAN_PATI_REASON, bool>>>();
    }
}
