using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCoTreatmentSO : StagingObjectBase
    {
        public HisCoTreatmentSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CO_TREATMENT, bool>>> listHisCoTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CO_TREATMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_CO_TREATMENT, bool>>> listVHisCoTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CO_TREATMENT, bool>>>();
    }
}
