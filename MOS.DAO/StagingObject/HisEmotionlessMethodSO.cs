using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEmotionlessMethodSO : StagingObjectBase
    {
        public HisEmotionlessMethodSO()
        {
            //listHisEmotionlessMethodExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_METHOD, bool>>> listHisEmotionlessMethodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_METHOD, bool>>>();
    }
}
