using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTreatmentLoggingSO : StagingObjectBase
    {
        public HisTreatmentLoggingSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_LOGGING, bool>>> listHisTreatmentLoggingExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_LOGGING, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_LOGGING, bool>>> listVHisTreatmentLoggingExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_LOGGING, bool>>>();
    }
}
