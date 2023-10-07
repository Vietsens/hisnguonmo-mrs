using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTreatmentTypeSO : StagingObjectBase
    {
        public HisTreatmentTypeSO()
        {
            //listHisTreatmentTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_TYPE, bool>>> listHisTreatmentTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_TYPE, bool>>>();
    }
}
