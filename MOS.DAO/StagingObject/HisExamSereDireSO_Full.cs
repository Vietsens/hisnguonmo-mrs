using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExamSereDireSO : StagingObjectBase
    {
        public HisExamSereDireSO()
        {
            //listHisExamSereDireExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisExamSereDireExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERE_DIRE, bool>>> listHisExamSereDireExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERE_DIRE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXAM_SERE_DIRE, bool>>> listVHisExamSereDireExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXAM_SERE_DIRE, bool>>>();
    }
}
