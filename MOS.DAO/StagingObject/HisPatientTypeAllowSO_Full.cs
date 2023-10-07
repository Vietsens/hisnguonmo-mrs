using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientTypeAllowSO : StagingObjectBase
    {
        public HisPatientTypeAllowSO()
        {
            //listHisPatientTypeAllowExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisPatientTypeAllowExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALLOW, bool>>> listHisPatientTypeAllowExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALLOW, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALLOW, bool>>> listVHisPatientTypeAllowExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALLOW, bool>>>();
    }
}
