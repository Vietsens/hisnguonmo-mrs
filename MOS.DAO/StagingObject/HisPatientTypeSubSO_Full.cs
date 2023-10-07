using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientTypeSubSO : StagingObjectBase
    {
        public HisPatientTypeSubSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_SUB, bool>>> listHisPatientTypeSubExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_SUB, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_SUB, bool>>> listVHisPatientTypeSubExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_SUB, bool>>>();
    }
}
