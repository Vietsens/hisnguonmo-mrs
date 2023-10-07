using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExamServiceTempSO : StagingObjectBase
    {
        public HisExamServiceTempSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERVICE_TEMP, bool>>> listHisExamServiceTempExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERVICE_TEMP, bool>>>();
    }
}
