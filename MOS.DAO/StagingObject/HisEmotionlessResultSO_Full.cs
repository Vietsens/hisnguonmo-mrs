using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEmotionlessResultSO : StagingObjectBase
    {
        public HisEmotionlessResultSO()
        {
            //listHisEmotionlessResultExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_RESULT, bool>>> listHisEmotionlessResultExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_RESULT, bool>>>();
    }
}
